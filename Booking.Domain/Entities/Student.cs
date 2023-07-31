namespace Booking.Domain.Entities
{
    public class Student : BaseEntity
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public List<MentorBooking>? Bookings { get; set; } = new();
    }
}