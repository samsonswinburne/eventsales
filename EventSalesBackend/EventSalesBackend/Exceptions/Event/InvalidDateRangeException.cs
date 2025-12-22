namespace EventSalesBackend.Exceptions.Event;

public class InvalidDateRangeException : Exception
{
    public InvalidDateRangeException(DateTime startDate, DateTime endDate)
        : base(
            $"Event end date ({endDate:yyyy-MM-dd HH:mm}) cannot be before start date ({startDate:yyyy-MM-dd HH:mm})")
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
}