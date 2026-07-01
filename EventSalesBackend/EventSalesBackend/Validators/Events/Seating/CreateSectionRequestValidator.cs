using System.Data;
using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Events.Seating;
using EventSalesBackend.Models.DTOs.Validators;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Validators.Events.Seating;

public class CreateSectionRequestValidator : AbstractValidator<CreateSectionValidatorDTO>
{
    private readonly IEventRepository _eventRepository;
    
    public CreateSectionRequestValidator(IEventRepository eventRepository, IUserClaimsService userClaimsService)
    {
        _eventRepository = eventRepository;
        
        
        RuleFor(x => x.Body.Name).NotEmpty().WithMessage("Name is required")
            .Matches(RegexPatterns.NamePattern).WithMessage(RegexValidationMessages.NamePatternMessage)
            .Length(1, 50).WithMessage("Name must be between 1 and 50 characters");
        // we don't need to check for seats being present or not present if its standing because we handle that later (not as validation just as the seats get ignored)
        RuleFor(x => x.Body.Seats)
            .Must(HaveUniqueValues)
            .WithMessage("Seats must be unique");
        RuleFor(x => x).CustomAsync(async (request, context, c) =>
        {
            var userId = userClaimsService.GetUserId();
            if (userId is null)
            {
                context.AddFailure("UserId", "Invalid user id");
                return;
            }

            if (!ObjectId.TryParse(request.Body.TicketTypeId, out var ticketTypeId))
            {
                context.AddFailure("TicketTypeId", "Invalid ticket type id");
                return; // Stop early if we cannot pass a valid ID to the database lookup
            }
            Console.WriteLine(request.EventId);
            if (!ObjectId.TryParse(request.EventId, out var eventId))
            {
                context.AddFailure("EventId", "Invalid event id");
                return; 
            }

            var isValid = await OwnsValidEventWithValidTicketType(userId, eventId, ticketTypeId);
            if (!isValid)
            {
                context.AddFailure("Event", "You do not have permission to modify this event, or the specified ticket type does not exist.");
            }
        });
    }

    private bool HaveUniqueValues(List<CreateSectionSeat>? seats)
    {
        if (seats == null)
        {
            return true;
        }
        var uniqueCount = seats
            .Select(s => new { s.Row, s.SeatNumber })
            .Distinct()
            .Count();
        return uniqueCount == seats.Count;

    }
    private async Task<bool> OwnsValidEventWithValidTicketType(string userId, ObjectId eventId, ObjectId ticketTypeId)
    {

        var filter = Builders<Event>.Filter.And(
            Builders<Event>.Filter.Eq(e => e.Id, eventId),
            Builders<Event>.Filter.AnyEq(e => e.Admins, userId), 
            Builders<Event>.Filter.ElemMatch(e => e.TicketTypes, t => t.Id == ticketTypeId)
        );

        var results = await _eventRepository.GetByFilter(filter);
        return results.Count == 1;
    }
}