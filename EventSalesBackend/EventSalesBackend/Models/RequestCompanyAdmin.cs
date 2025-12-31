using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.CompilerServices;

namespace EventSalesBackend.Models
{
    public class RequestCompanyAdmin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public required ObjectId CompanyId { get; init; }
        public required string RequestSenderId { get; init; }
        public required string RequestReceiverId { get; init; }
    }
    public static class RequestCompanyAdminExtensions
    {
        public static RequestCompanyAdminPublic ToPublic(this RequestCompanyAdmin rca)
        {
            return new RequestCompanyAdminPublic
            {
                Id = rca.Id.ToString(),
                CompanyId = rca.CompanyId.ToString(),
                RequestReceiverId = rca.RequestReceiverId,
                RequestSenderId = rca.RequestSenderId
            };
        }
    }
}
