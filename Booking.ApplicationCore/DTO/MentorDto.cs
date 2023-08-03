namespace Booking.ApplicationCore.DTO
{
    public class MentorDto 
    {
        public string MentorId { get; set; }

        public string Name { get; set; }

        public string Biography { get; set; }

        public double HourlyRate { get; set; }

        public int MeetingDuration { get; set; }

        public string CategoryId { get; set; }

        public List<AvailabilityDto> Availabilities { get; set; } = new();
    }
}