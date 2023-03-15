namespace TestAuth.Domain.Model.Exceptions
{
    [Serializable]
    public class NotFoundAppException : AppException
    {
        public NotFoundAppException()
            : this("Not found.", "The specified resource was not found.")
        { }

        public NotFoundAppException(string title, string detail, IDictionary<string, object>? extensions = null, Exception? innerException = null)
            : base(title, detail, extensions, innerException) { }

        protected NotFoundAppException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
