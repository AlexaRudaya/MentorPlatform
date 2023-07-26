namespace Mentors.Domain.Entities
{
    public class Availability : BaseEntity
    {
        public required DateTime Date { get; set; }

        public required bool IsAvailable { get; set; }

        public required DateTime StartTime { get; set; }

        public required DateTime EndTime { get; set; }

        public Guid MentorId { get; set; }

        public Mentor? Mentor { get; set; }
    }
}