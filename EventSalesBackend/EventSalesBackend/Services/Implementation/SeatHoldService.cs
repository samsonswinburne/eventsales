using EventSalesBackend.Exceptions.Event;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data.Seat;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Implementation;

public class SeatHoldService : ISeatHoldService
{
    private readonly ISeatHoldRepository _seatHoldRepository;
    private readonly IEventService _eventService;
    public SeatHoldService(ISeatHoldRepository seatHoldRepository, IEventService eventService)
    {
        _seatHoldRepository = seatHoldRepository;
        _eventService = eventService;
    }
    public async Task<SeatHoldData> Create(string userId, SeatHoldIdentifier seatHoldIdentifier, CancellationToken cancellationToken)
    {
        var @event = await _eventService.GetById(seatHoldIdentifier.EventId);
        if (@event == null)
        {
            throw new EventNotFoundException(seatHoldIdentifier.EventId);
        }
        ObjectId? ticketTypeId = @event.Sections.FirstOrDefault(s => s.Id ==  seatHoldIdentifier.SectionId)?.TicketTypeId;
        if (ticketTypeId == null)
        {
            throw new InvalidOperationException("section not found");
        }

        var seatHold = new SeatHold
        {
            EventId = seatHoldIdentifier.EventId,
            SectionId = seatHoldIdentifier.SectionId,
            UserId = userId,
            Row = seatHoldIdentifier.Row,
            SeatNumber =  seatHoldIdentifier.SeatNumber,
            TicketTypeId = ticketTypeId.Value,
            ExpiresAt = DateTime.UtcNow.AddMinutes(2)
        };
        try
        {
            return await _seatHoldRepository.Insert(seatHold, cancellationToken);
        }
        catch (MongoWriteException ex)
        {
            Console.WriteLine(ex);
            
        }

        return null;
    }

    public async Task<List<SeatHold>> FindByUserId(string userId, CancellationToken cancellationToken)
    {
        return await _seatHoldRepository.GetSeatHolds(userId, cancellationToken);
    }

    public async Task<bool> UpdateManyByUserIdAsync(string userId, UpdateDefinition<SeatHold> update, IClientSessionHandle session,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}