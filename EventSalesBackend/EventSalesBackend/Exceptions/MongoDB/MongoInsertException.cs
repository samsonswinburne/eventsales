using EventSalesBackend.Extensions;

namespace EventSalesBackend.Exceptions.MongoDB
{
    public class MongoInsertException : Exception, BaseException
    {
        private string _type;
        public MongoInsertException(string type) : base()
        { 
            _type = type;
        }

        public object ToErrorResponse()
        {
            return new
            {
                errors = new
                {
                    type = $"{_type} was not able to be created"
                }
            };
        }

    }
}
