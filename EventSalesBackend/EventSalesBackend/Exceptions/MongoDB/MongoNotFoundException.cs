using EventSalesBackend.Extensions;

namespace EventSalesBackend.Exceptions.MongoDB
{
    public class MongoNotFoundException : Exception, BaseException
    {
        private readonly string _type;
        private readonly string? _id;
        public MongoNotFoundException(string type) : base($"Finding {type} failed.")
        {
            _type = type;
        }
        public MongoNotFoundException(string type, string id) : base($"Finding {type} with id {id} failed.")
        {
            _type=type;
            _id = id;
        }

        public object ToErrorResponse()
        {
            switch (_id)
            {
                case (null):
                    return new
                    {
                        errors = new
                        {
                            type = $"{_type} was not found"
                        }
                    };
                default:
                    return new
                    {
                        errors = new
                        {
                            type = $"{_type} was not found",
                            id = $"a {_type} with id {_id} was not found"
                        }
                    };
            }
            
        }
    }
}
