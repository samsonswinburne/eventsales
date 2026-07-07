using EventSalesBackend.Extensions;
using EventSalesBackend.Models;

namespace EventSalesBackend.Exceptions.Users
{
    public class IncorrectUserAccountTypeException : BaseException
    {
        private AccountType _expected;
        private AccountType _actual;
        public IncorrectUserAccountTypeException(AccountType expectedType, AccountType actualType)
        {
            _expected = expectedType;
            _actual = actualType;
        }

        public object ToErrorResponse()
        {
            return new
            {
                errors = new
                {
                    accountType = $"Expected account type of {_expected}, actual type of {_actual}"
                }
            };
        }
    }
}
