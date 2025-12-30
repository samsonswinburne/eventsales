using System.Text.Json.Serialization;

namespace EventSalesBackend.Models.DTOs.Data.GoogleJSON
{
    public class ReverseGeocodeParent
    {
        [JsonPropertyName("results")]
        public List<GeocodeJsonAddress> Addresses { get; init; }
        [JsonPropertyName("status")]
        public string Status { get; init; }
    }
}
