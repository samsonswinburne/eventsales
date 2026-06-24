using System.ComponentModel.DataAnnotations;

namespace EventSalesBackend.Models.DTOs.Response.PublicInfo;

public class VenuePublic
{
    
}
public class JsonVenueLocation
{
    [Required] [Range(-180, 180)] public required double Latitude { get; set; }

    [Required] [Range(-90, 90)] public required double Longitude { get; set; }
}
