using EventSalesBackend.Pipelines.Interfaces;
using MongoDB.Driver;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace EventSalesBackend.Pipelines.Implementation;

public class MongoResiliencePipelineProvider : IMongoResiliencePipelineProvider
{
    public ResiliencePipeline Read { get; }
    public ResiliencePipeline Write { get; }
    public ResiliencePipeline Aggregation { get; }

    public MongoResiliencePipelineProvider(ILogger<MongoResiliencePipelineProvider> logger)
    {
        Read = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(100),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder()
                    .Handle<MongoConnectionException>()
                    .Handle<TimeoutException>(),
                OnRetry = args =>
                {
                    logger.LogWarning(
                        "MongoDB read retry {Attempt}: {Exception}",
                        args.AttemptNumber,
                        args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            })
            .AddTimeout(TimeSpan.FromSeconds(10))
            .Build();

        Write = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Linear,
                ShouldHandle = new PredicateBuilder()
                    .Handle<MongoConnectionException>(),
                OnRetry = args =>
                {
                    logger.LogWarning(
                        "MongoDB write retry {Attempt}: {Exception}",
                        args.AttemptNumber,
                        args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(30),
                OnOpened = args =>
                {
                    logger.LogError("MongoDB circuit breaker opened");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        Aggregation = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder()
                    .Handle<MongoConnectionException>()
                    .Handle<TimeoutException>()
            })
            .AddTimeout(TimeSpan.FromSeconds(30))
            .Build();
    }
}