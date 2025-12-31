namespace EventSalesBackend.Models.DTOs.Response.PublicInfo
{
    public class RequestCompanyAdminPublic
    {
        public required string Id { get; init; }
        public required string CompanyId { get; init; }
        public required string RequestSenderId { get; init; }
        public required string RequestReceiverId { get; init; }
                                
    }
}
