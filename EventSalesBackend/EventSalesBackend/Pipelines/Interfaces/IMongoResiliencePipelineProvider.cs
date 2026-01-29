using Polly;

namespace EventSalesBackend.Pipelines.Interfaces;

public interface IMongoResiliencePipelineProvider
{
    ResiliencePipeline Read { get; }
    ResiliencePipeline Write { get; }
    ResiliencePipeline Aggregation { get; }
    
}