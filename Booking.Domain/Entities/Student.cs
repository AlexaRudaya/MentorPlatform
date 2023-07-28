namespace Booking.Domain.Entities
{
    public class Student : BaseEntity
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public List<Bookings>? Bookings { get; set; } = new();
    }
}