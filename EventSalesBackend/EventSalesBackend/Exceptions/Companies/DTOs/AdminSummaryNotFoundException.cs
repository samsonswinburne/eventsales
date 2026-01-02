using EventSalesBackend.Extensions;
using MongoDB.Bson;

namespace EventSalesBackend.Exceptions.Companies.DTOs;

public class AdminSummaryNotFoundException : Exception, BaseException
{
    private readonly string _companyId;
    public AdminSummaryNotFoundException(ObjectId companyId) : base($"Company {companyId.ToString()} not found")
    {
        _companyId = companyId.ToString();
    }
    public object ToErrorResponse()
    {
        return new
        {
            errors = new
            {
                companyId = $"{_companyId} was not found"
            }
        };
    }
}