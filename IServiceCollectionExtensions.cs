using Polly.CircuitBreaker;

namespace CircuitBreakerChecks;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApiCircuitBreakerHealthCheck(
        this IServiceCollection services,
        string url,
        string name,
        AsyncCircuitBreakerPolicy policy)
    {
        services.AddTransient(_ => new ApiCircuitBreakerHealthCheckConfig { Url = url });
        services.AddSingleton(new ApiCircuitBreakerContainer(policy));
        services.AddHealthChecks().AddCheck<ApiCircuitBreakerHealthCheck<ApiCircuitBreakerContainer>>(name);
        return services;
    }
}
