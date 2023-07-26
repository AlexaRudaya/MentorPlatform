namespace Mentors.ApplicationCore.Exceptions
{
    public class MentorNotFoundException : ObjectNotFoundException
    {
        private static readonly string MentorsNotFoundMessage = "No mentors were found";
        private static readonly string MentorNotFoundMessage = "Mentor with such Id {0} was not found";
        public Guid MentorId { get; }

        public MentorNotFoundException() : base(MentorsNotFoundMessage)
        {
        }

        public MentorNotFoundException(Guid mentorId) : base(string.Format(MentorNotFoundMessage, mentorId))
        {
            MentorId = mentorId;
        }

        public MentorNotFoundException(string message) : base(message)
        {
        }
    }
}