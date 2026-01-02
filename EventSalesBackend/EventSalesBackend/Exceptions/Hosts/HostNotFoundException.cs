using EventSalesBackend.Extensions;

namespace EventSalesBackend.Exceptions.Hosts
{
    public class HostNotFoundException :Exception, BaseException
    {
        private string _email;
        public HostNotFoundException(string email) : base($"{email} was not found")
        {
            _email = email;
        }
        

        public object ToErrorResponse()
        {
            return new
            {
                errors = new
                {
                    email = $"{_email} was not found"
                }
            };
        }
        
    }
}
