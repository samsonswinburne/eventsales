using EventSalesBackend.Models.DTOs.Data.GoogleJSON;

namespace EventSalesBackend.Services.Interfaces
{
    public interface IGeocodeService
    {
        public Task<AddressPostCodeDTO> ReverseGeocode(double latitude, double longitude);
        public Task<double> Geocode(string address);
    }
}
