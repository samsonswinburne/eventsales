namespace EventSalesBackend.Exceptions.Geocode
{
    public class InvalidCountryException : Exception
    {
        public InvalidCountryException() : base("No country found for given point") 
        { }
        public InvalidCountryException(string country) : base($"Provided point is in {country}, not Australia")
        { }
    }
}
