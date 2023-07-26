namespace Mentors.Domain.Entities
{
    public sealed class Mentor : BaseEntity
    {
        public required string Name { get; set; }

        public required string Biography { get; set; }

        public required double HourlyRate { get; set; }

        public required int MeetingDuration { get; set; }

        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }

        public List<Availability> Availabilities { get; set; } = new();
    }
}