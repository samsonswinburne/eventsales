using EventSalesBackend.Models.DTOs.Request.Events.Seating;
using Microsoft.AspNetCore.Mvc;

namespace EventSalesBackend.Models.DTOs.Validators;

public class CreateSectionValidatorDTO
{
    [FromRoute] public string EventId { get; set; }
    [FromBody] public CreateSectionRequest Body { get; set; }
}