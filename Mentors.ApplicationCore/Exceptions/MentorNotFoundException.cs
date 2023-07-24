namespace Mentors.ApplicationCore.Exceptions
{
    public sealed class MentorNotFoundException : Exception
    {
        public MentorNotFoundException(string message) : base(message)
        {              
        }
    }
}