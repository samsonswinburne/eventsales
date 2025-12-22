namespace EventSalesBackend.Exceptions.Configuration;

public class EventSalesMongoConfigurationException : Exception
{
    public EventSalesMongoConfigurationException(string field) : base(
        "field {field} is not set. MongoDB will not connect")
    {
    }
}