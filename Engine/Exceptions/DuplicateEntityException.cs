namespace Engine.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string message) : base(message) { }

        public DuplicateEntityException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
