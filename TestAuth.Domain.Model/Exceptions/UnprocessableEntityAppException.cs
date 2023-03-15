using System.Runtime.Serialization;

namespace TestAuth.Domain.Model.Exceptions
{
    [Serializable]
    public class UnprocessableEntityAppException : AppException
    {
        public UnprocessableEntityAppException()
            : this("Unprocessable Entity.", "The server understands the content type of the request object " +
                                            "and the syntax of the request object is correct, " +
                                            "but it was unable to process the contained instructions.")
        { }

        public UnprocessableEntityAppException(string title, string detail, IDictionary<string, object>? extensions = null, Exception? innerException = null)
            : base(title, detail, extensions, innerException) { }

        protected UnprocessableEntityAppException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

}
