using System.ComponentModel.DataAnnotations;

namespace EventSalesBackend.Models.DTOs.Request.Events;

public class GetNearbyEventsRequest
{
    [Required] public double Latitude { get; set; }

    [Required] public double Longitude { get; set; }

    [Required] [Range(0, 10000)] public double Radius { get; set; }

    public double Page { get; set; } = 0;
}