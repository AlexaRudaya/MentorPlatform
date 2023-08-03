namespace Booking.ApplicationCore.DTO
{
    public class AvailabilityDto
    {
        public string MentorId { get; set; }

        public DateTime Date { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}