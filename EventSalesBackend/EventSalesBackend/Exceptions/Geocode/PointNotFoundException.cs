namespace EventSalesBackend.Exceptions.Geocode
{
    public class PointNotFoundException : Exception
    {
        public PointNotFoundException() : base("Latitude / Longitude returned nothing")
        { }
    }
}
