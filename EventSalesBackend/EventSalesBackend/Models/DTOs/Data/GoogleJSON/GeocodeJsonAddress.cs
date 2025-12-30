using System.Text.Json.Serialization;

namespace EventSalesBackend.Models.DTOs.Data.GoogleJSON
{
    public class GeocodeJsonAddress
    {
        [JsonPropertyName("address_components")]
        public List<GeocodeAddressComponent> Components { get; init; }
    }
}
