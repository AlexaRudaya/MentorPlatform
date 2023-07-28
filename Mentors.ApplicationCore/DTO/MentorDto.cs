namespace Mentors.ApplicationCore.DTO
{
    public class MentorDto : BaseDto
    {
        public string? Name { get; set; }

        public string? Biography { get; set; }

        public double HourlyRate { get; set; }

        public int MeetingDuration { get; set; }

        public Guid CategoryId { get; set; }

        public List<Guid> AvailabilitiesIds { get; set; } = new();
    }
}