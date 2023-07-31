namespace Booking.ApplicationCore.DTO
{
    public class BookingDto : BaseDto  
    {
        public DateTime StartTimeBooking { get; set; }

        public DateTime EndTimeBooking { get; set; }

        public Guid StudentId { get; set; }

        public string MentorId { get; set; }
    }
}