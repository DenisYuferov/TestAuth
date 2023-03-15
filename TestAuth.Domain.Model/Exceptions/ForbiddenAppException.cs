namespace TestAuth.Domain.Model.Exceptions
{
    [Serializable]
    public class ForbiddenAppException : AppException
    {
        public ForbiddenAppException()
            : this("Forbidden.", "Not enough Permissions.")
        { }

        public ForbiddenAppException(string title, string detail, IDictionary<string, object>? extensions = null, Exception? innerException = null)
            : base(title, detail, extensions, innerException) { }

        protected ForbiddenAppException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
