using System.Text.Json.Serialization;

namespace EventSalesBackend.Models.DTOs.Data.GoogleJSON
{
    public class GeocodeAddressComponent
    {
        [JsonPropertyName("short_name")]
        public string ShortName { get; init; }
        [JsonPropertyName("types")]
        public List<string> Types { get; init; }
    }
}
