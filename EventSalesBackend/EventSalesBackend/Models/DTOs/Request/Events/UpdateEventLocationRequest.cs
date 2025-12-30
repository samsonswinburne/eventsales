namespace EventSalesBackend.Models.DTOs.Request.Events
{
    public class UpdateEventLocationRequest
    {
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    }
}
