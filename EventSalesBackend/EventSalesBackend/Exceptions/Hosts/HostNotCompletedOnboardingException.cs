using EventSalesBackend.Extensions;

namespace EventSalesBackend.Exceptions.Hosts
{
    public class HostNotCompletedOnboardingException : Exception, BaseException
    {
        private readonly string? _hostId;
        public HostNotCompletedOnboardingException() : base("Host has not completed onboarding yet")
        {

        }
        public HostNotCompletedOnboardingException(string hostId) : base("Host has not completed onboarding yet")
        {
            _hostId = hostId;
        }
        public object ToErrorResponse()
        {
            switch (_hostId)
            {
                case not null:
                    return new
                    {
                        errors = new
                        {
                            onboarding = $"host {_hostId} has not completed onboarding"
                        }
                    };
                case null:
                    return new
                    {
                        errors = new
                        {
                            onboarding = $"host has not completed onboarding"
                        }
                    };
            }
        }
    }
}
