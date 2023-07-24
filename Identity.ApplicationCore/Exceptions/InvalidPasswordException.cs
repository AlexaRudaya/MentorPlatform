namespace Identity.ApplicationCore.Exceptions
{
    public sealed class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string message) : base(message)
        {
        }
    }
}