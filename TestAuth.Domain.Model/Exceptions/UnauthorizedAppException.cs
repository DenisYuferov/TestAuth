namespace TestAuth.Domain.Model.Exceptions
{
    [Serializable]
    public class UnauthorizedAppException : AppException
    {
        public UnauthorizedAppException()
            : this("Unauthorized.", "Lacks valid authentication credentials.")
        { }

        public UnauthorizedAppException(string title, string detail, IDictionary<string, object>? extensions = null, Exception? innerException = null)
            : base(title, detail, extensions, innerException) { }

        protected UnauthorizedAppException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
