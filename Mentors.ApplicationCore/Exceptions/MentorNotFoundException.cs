namespace Mentors.ApplicationCore.Exceptions
{
    public class MentorNotFoundException : ObjectNotFoundException
    {
        private static readonly string _mentorsNotFoundMessage = "No mentors were found";
        private static readonly string _mentorNotFoundMessage = "Mentor with such Id {0} was not found";
        public Guid MentorId { get; }

        public MentorNotFoundException() : base(_mentorsNotFoundMessage)
        {
        }

        public MentorNotFoundException(Guid mentorId) : base(string.Format(_mentorNotFoundMessage, mentorId))
        {
            MentorId = mentorId;
        }
    }
}