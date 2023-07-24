namespace Mentors.Domain.Entities
{
    public class Mentor : BaseEntity
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Biography { get; set; }

        [Required]
        public double HourlyRate { get; set; }

        [Required]
        public int MeetingDuration { get; set; }

        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }

        public List<Availability>? Availabilities { get; set; } = new();
    }
}