namespace Booking.Domain.Entities
{
    public class Bookings : BaseEntity
    {
        public required DateTime StartTimeBooking { get; set; }

        public required DateTime EndTimeBooking { get; set; }

        public Guid StudentId { get; set; }

        public Student? Student { get; set; }

        public Guid MentorId { get; set; }
    }
}