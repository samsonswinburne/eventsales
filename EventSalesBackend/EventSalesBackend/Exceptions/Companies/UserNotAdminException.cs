using EventSalesBackend.Extensions;
using MongoDB.Bson;

namespace EventSalesBackend.Exceptions.Companies;

public class UserNotAdminException : Exception, BaseException
{
    private readonly string _userId;
    private readonly ObjectId? _companyId;

    public UserNotAdminException(string userId) : base($"{userId} is not an admin of this company")
    {
        _userId = userId;
    }

    public UserNotAdminException(string userId, ObjectId companyId) : base(
        $"{userId} is not an admin of {companyId.ToString()}")
    {
        _userId = userId;
        _companyId = companyId;
    }
    

    public object ToErrorResponse()
        {
            switch (_companyId)
            {
                case null:
                    return new
                    {
                        errors = new
                        {
                            userId = $"{_userId} is not an admin of this company"
                        }
                    };
                default:
                    return new
                    {
                        errors = new
                        {
                            userId = $"{_userId} is not an admin of {_companyId}"
                        }
                    };
            }
            
        }
}