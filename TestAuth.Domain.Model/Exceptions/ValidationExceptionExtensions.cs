using FluentValidation;

namespace TestAuth.Domain.Model.Exceptions
{
    public static class ValidationExceptionExtensions
    {
        public static IDictionary<string, object> GetErrorsDictionary(this ValidationException exception)
        {
            var errorsDictionary = new Dictionary<string, object>();

            foreach (var error in exception.Errors!)
            {
                errorsDictionary.TryAdd(error.PropertyName, error.ErrorMessage);
            }

            return errorsDictionary;
        }
    }
}
