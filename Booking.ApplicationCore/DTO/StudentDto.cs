namespace Booking.ApplicationCore.DTO
{
    public class StudentDto : BaseDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public List<Guid> BookingsIds { get; set; } = new();
    }
}