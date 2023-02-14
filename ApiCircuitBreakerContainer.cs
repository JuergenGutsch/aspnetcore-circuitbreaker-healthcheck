using Polly.CircuitBreaker;

namespace CircuitBreakerChecks;

public class ApiCircuitBreakerContainer
{
    private readonly AsyncCircuitBreakerPolicy _policy;
    public ApiCircuitBreakerContainer(AsyncCircuitBreakerPolicy policy)
    {
        _policy = policy;
    }
    public AsyncCircuitBreakerPolicy Policy => _policy;
}