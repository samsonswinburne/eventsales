namespace EventSalesBackend.Exceptions.Event;

public class EventDateInPastException : Exception
{
    public EventDateInPastException(DateTime providedDate)
        : base($"Event date ({providedDate:yyyy-MM-dd HH:mm}) cannot be in the past")
    {
        ProvidedDate = providedDate;
    }

    public DateTime ProvidedDate { get; }
}