namespace Booking.Domain.Entities
{
    public class Bookings : BaseEntity
    {
        public required DateTime StartTimeBooking { get; set; }

        public required DateTime EndTimeBooking { get; set; }

        public int StudentId { get; set; }

        public Student? Student { get; set; }

        public int MentorId { get; set; }
    }
}