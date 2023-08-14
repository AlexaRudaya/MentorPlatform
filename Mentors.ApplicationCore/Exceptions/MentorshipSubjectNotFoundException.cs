namespace Mentors.ApplicationCore.Exceptions
{
    public class MentorshipSubjectNotFoundException : ObjectNotFoundException
    {
        private static readonly string _subjectsNotFoundMessage = "No subjects were found";
        private static readonly string _subjectNotFoundMessage = "Subject with such Id {0} was not found";
        public Guid MentorshipSubjectId { get; }

        public MentorshipSubjectNotFoundException() : base(_subjectsNotFoundMessage)
        {
        }

        public MentorshipSubjectNotFoundException(Guid mentorshipSubjectId) : base(string.Format(_subjectNotFoundMessage, mentorshipSubjectId))
        {
            MentorshipSubjectId = mentorshipSubjectId;
        }
    }
}