using EventSalesBackend.Extensions;

namespace EventSalesBackend.Exceptions.Event
{
    public class SlugNotFoundException : Exception, BaseException
    {
        private readonly string _slug;
        public SlugNotFoundException(string slug) : base($"Event with slug {slug} was not found")
        {
            _slug = slug;
        }
        public object ToErrorResponse()
        {
            return new
            {
                errors = new
                {
                    slug = $"Event with slug {_slug} was not found"
                }
            };
        }
    }
}
