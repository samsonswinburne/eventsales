namespace EventSalesBackend.Exceptions;

public class EventDateInPastException : Exception
{
    public DateTime ProvidedDate { get; }

    public EventDateInPastException(DateTime providedDate) 
        : base($"Event date ({providedDate:yyyy-MM-dd HH:mm}) cannot be in the past")
    {
        ProvidedDate = providedDate;

    }
    
}