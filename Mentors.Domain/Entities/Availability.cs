namespace Mentors.Domain.Entities
{
    public class Availability : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public Guid MentorId { get; set; }

        public Mentor? Mentor { get; set; }
    }
}