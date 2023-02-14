using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CircuitBreakerChecks;

public class ApiCircuitBreakerHealthCheck<TPolicy> : IHealthCheck where TPolicy : ApiCircuitBreakerContainer
{
    private readonly ApiCircuitBreakerHealthCheckConfig _config;
    private readonly IServiceProvider _services;

    public ApiCircuitBreakerHealthCheck(
        ApiCircuitBreakerHealthCheckConfig config, 
        IServiceProvider services)
    {
        _config = config;
        _services = services;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var policy = _services.GetService<TPolicy>()?.Policy;

        try
        {
            if (policy is not null)
            {
                await policy.ExecuteAsync(async () =>
                {
                    var client = new HttpClient();
                    var response = await client.GetAsync(_config.Url);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException();
                    }
                });
            }
        }
        catch (Exception)
        {
            return HealthCheckResult.Unhealthy("Unhealthy");
        }

        return HealthCheckResult.Healthy("Healthy");
    }
}
