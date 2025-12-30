namespace EventSalesBackend.Exceptions.Configuration
{
    public class GoogleApiKeyNotFoundException : Exception
    {
        public GoogleApiKeyNotFoundException() : base("The Google:ApiKey was not found in the configuration file") { }
    }
}
