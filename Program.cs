using CircuitBreakerChecks;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddApiCircuitBreakerHealthCheck(
    "http://localhost:5259/api/dummy", // URL to check
    "AddApiCircuitBreakerHealthCheck", // Name of the health check registration
    Policy.Handle<HttpRequestException>() // Polly CircuitBreaker Async Policy
        .CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: 2,
            durationOfBreak: TimeSpan.FromMinutes(1)
        ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseHealthChecks("/health");

app.MapGet("/api/dummy", () =>
{
    var i = Random.Shared.NextInt64(0, 1000);
    if ((i % 5) == 0)
    {
        throw new Exception("new exception");
    }
    return i;
});
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
