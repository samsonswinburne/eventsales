using EventSalesBackend.Exceptions.Configuration;
using EventSalesBackend.Exceptions.Geocode;
using EventSalesBackend.Models.DTOs.Data.GoogleJSON;
using EventSalesBackend.Services.Interfaces;

namespace EventSalesBackend.Services.Implementation
{
    public class GeocodeService : IGeocodeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        public GeocodeService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            var googleConfig = config.GetRequiredSection("Google");
            _apiKey = googleConfig["ApiKey"] ?? throw new GoogleApiKeyNotFoundException();
        }
        private const string _rvgeocodestring = "https://maps.googleapis.com/maps/api/geocode/json?latlng=";
        private static AddressPostCodeDTO ReverseGeocodeToString(ReverseGeocodeParent parent)
        {
            if(parent.Status == "ZERO_RESULTS")
            {
                // failed
                throw new PointNotFoundException();
                
            }
            if(parent.Status == "INVALID_REQUEST")
            {
                throw new Exception("Invalid Request, point doesn't work. this shouldn't occur validation needs an update ");
            }
            if(parent.Status == "OK")
            {
                var components = parent.Addresses[0].Components;

                if (components[components.Count-2].ShortName != "AU")
                {
                    throw new InvalidCountryException(components[components.Count - 2].ShortName);
                }

                var addressString = components[0].ShortName + " " + components[1].ShortName + ", " + components[2].ShortName;
                var postCode = components[components.Count - 1].ShortName;

                return new AddressPostCodeDTO
                {
                    Address = addressString,
                    PostCode = postCode
                };
            }
            throw new Exception("This shouldnt occur, Reverse Geocode to string function, some error has occured");
        }
        public async Task<double> Geocode(string address)
        {
            throw new NotImplementedException();
        }

        public async Task<AddressPostCodeDTO> ReverseGeocode(double latitude, double longitude)
        {
            
            string queryString = _rvgeocodestring + latitude.ToString() + ", " + longitude.ToString()
                + "&result_type=street_address&key=" + _apiKey;
            var client = _httpClientFactory.CreateClient("rvGeocodeClient");
            var result = await client.GetFromJsonAsync<ReverseGeocodeParent>(queryString);
            if (result is null)
            {
                throw new PointNotFoundException();
            }

            return ReverseGeocodeToString(result);
            
        }
    }
}
