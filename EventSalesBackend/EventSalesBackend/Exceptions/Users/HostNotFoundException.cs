using EventSalesBackend.Extensions;

namespace EventSalesBackend.Exceptions.Hosts
{
    public class HostNotFoundException :Exception, BaseException
    {
        private string? _email;
        private string? _userId;

        

        public HostNotFoundException(string? email = null, string? userId = null) : base(email is not null ? $"A user with email {email} was not found" : $"A user with Id {userId} was not found. Have you completed onboarding?")

        {
            _email = email;
            _userId = userId;

            if(_email is null && _userId is null)
            {
                throw new ArgumentNullException("email, userId cannot both be null");
            }
        }
        

        public object ToErrorResponse()
        {
            switch (_email)
            {
                case null:
                    return new
                    {
                        errors = new
                        {
                            userId = $"A user with Id {_userId} was not found. Have you completed onboarding?"
                        }
                    };

                case not null:
                    return new
                    {
                        errors = new
                        {
                            email = $"A user with email {_email} was not found"
                        }
                    };
            }
            
        }
        
    }
}
