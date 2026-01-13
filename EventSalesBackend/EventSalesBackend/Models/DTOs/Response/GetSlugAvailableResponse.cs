namespace EventSalesBackend.Models.DTOs.Response
{
    public class GetSlugAvailableResponse
    {
        public required string Slug { get; init; }
        public required bool Available { get; init; }
    }
}
