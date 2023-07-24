namespace Mentors.ApplicationCore.DTO
{
    public class MentorCreateDto : BaseDto
    {
        public string? Name { get; set; }

        public string? Biography { get; set; }

        public double HourlyRate { get; set; }

        public int MeetingDuration { get; set; }

        public Guid CategoryId { get; set; }

        public List<AvailabilityDto>? Availabilities { get; set; } = new();
    }
}