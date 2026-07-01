using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Implementation;

public class CheckoutService : ICheckoutService
{
    private readonly ISeatHoldService _seatHoldService;
    private readonly IPayPalService _payPalService;
    private readonly ITicketService _ticketService;
    private readonly ISessionProvider _sessionProvider;
    private readonly IEventRepository _eventService;

    public CheckoutService(ISeatHoldService seatHoldService,  IPayPalService payPalService, 
        ITicketService ticketService, ISessionProvider sessionProvider, IEventRepository eventService
        )
    {
        _seatHoldService = seatHoldService;
        _payPalService = payPalService;
        _ticketService = ticketService;
        _sessionProvider = sessionProvider;
       _eventService = eventService;
    }
    
    public async Task<List<Ticket>> CreateOrderAsync(string userId, string email, string name, CancellationToken cancellationToken)
    {
        // check if stuff exists and extend it!
        // current defined behaviour is attempt all if 1 fails fail order
        var session = await _sessionProvider.StartSessionAsync(cancellationToken);
        session.StartTransaction();
        
        var seatHolds = await _seatHoldService.FindByUserId(userId, cancellationToken);
        
        // for now we will enforce holding only a lock in 1 event at a time

        if (seatHolds.Count == 0)
        {
            // no seats are found, realising now that list is a poor type that i have been using haha
            return [];
        }
        var eventIds = seatHolds.Select(s => s.EventId).Distinct().ToList();
        if (eventIds.Count > 1)
        {
            throw new InvalidOperationException("The seats selected are from many different events");
        }
        CryptoService cryptoService = new CryptoService();
        //var ticketTypes = await _eventService.getTicketTypes(eventIds[0], cancellationToken);
        var ticketTypes = new List<TicketType>();
        var tickets = new List<Ticket>();
        foreach (var seatHold in seatHolds)
        {
            TicketType type = ticketTypes.FirstOrDefault(tt => tt.Id == seatHold.TicketTypeId);
            if (type is null)
            {
                throw new InvalidOperationException("Duplicate ticket types found");
            }

            Seat seatToAdd = new Seat
            {
                EventId = seatHold.EventId,
                SeatNumber = seatHold.SeatNumber,
                Row = seatHold.Row,
                SectionId = seatHold.SectionId,
                TicketTypeId = type.Id,
            };
            tickets.Add(
                new Ticket
                {
                    EventId = seatHold.EventId,
                    TicketTypeId = type.Id,
                    CustomerEmail = email,
                    CustomerName = name,
                    OrderDelivered = false,
                    OrderDeliveryDate = null,
                    CustomerId = userId,
                    PurchasePrice = type.TotalPrice,
                    OriginalPrice = type.TotalPrice,
                    Key = cryptoService.GenerateKey(),
                    Seat = seatToAdd
                }
                );
        }
        var result = await _ticketService.InsertMany(tickets, session, cancellationToken);
        // we don't actually need a transaction for this after all haha!
        if (result)
            await session.CommitTransactionAsync(cancellationToken);
        else
        {
            await session.AbortTransactionAsync(cancellationToken);
        }
        return tickets;
    }
}