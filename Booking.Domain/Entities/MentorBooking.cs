namespace Booking.Domain.Entities
{
    public class MentorBooking : BaseEntity
    {
        public required DateTime StartTimeBooking { get; set; }

        public required DateTime EndTimeBooking { get; set; }

        public Guid StudentId { get; set; }

        public Student? Student { get; set; }

        public string MentorId { get; set; }
    }
}