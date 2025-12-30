namespace EventSalesBackend.Exceptions.Event;

public class InvalidEventIdException : Exception
{
    public InvalidEventIdException(string eventId) : base($"The event Id  {eventId} is invalid."){}
}