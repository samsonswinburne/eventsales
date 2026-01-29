using EventSalesBackend.Models;
using EventSalesBackend.Pipelines.Interfaces;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Implementation;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMongoResiliencePipelineProvider _pipelines;
    
    public TicketService(ITicketRepository ticketRepository, IMongoResiliencePipelineProvider pipelineProvider)
    {
        _ticketRepository = ticketRepository;
        _pipelines = pipelineProvider;
    }
    public async Task<TicketStatus> GetTicketStatusFromKey(string key, string scannerId, CancellationToken cancellationToken)
    {
        return await _pipelines.Read.ExecuteAsync(async ct =>
            {
                return await _ticketRepository.GetStatusFromKeyProtected(key, scannerId, ct);
            }, cancellationToken
        );
    }

    public async Task Get(ObjectId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}