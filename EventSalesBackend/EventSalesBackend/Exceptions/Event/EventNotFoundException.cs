using EventSalesBackend.Extensions;
using MongoDB.Bson;

namespace EventSalesBackend.Exceptions.Event
{
    public class EventNotFoundException : Exception, BaseException
    {
        private readonly string _id;
        public EventNotFoundException(ObjectId id) : base($"Event with id {id.ToString()} was not found")
        {
            _id = id.ToString();
        }
        public EventNotFoundException() : base($"Event was not found")
        {
            _id = ObjectId.Empty.ToString();
        }
        public object ToErrorResponse()
        {
            return new
            {
                errors = new
                {
                    id = $"Event with id {_id} was not found"
                }
            };
        }
    }
}
