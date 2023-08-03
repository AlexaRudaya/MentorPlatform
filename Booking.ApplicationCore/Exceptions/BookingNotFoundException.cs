namespace Booking.ApplicationCore.Exceptions
{
    public class BookingNotFoundException : ObjectNotFoundException
    {
        private static readonly string _bookingsNotFoundMessage = "No bookings were found";
        private static readonly string _bookingNotFoundMessage = "Booking with such Id {0} was not found";
        public Guid BookingId { get; }

        public BookingNotFoundException() : base(_bookingsNotFoundMessage)
        {
        }

        public BookingNotFoundException(Guid bookingId) : base(string.Format(_bookingNotFoundMessage, bookingId))
        {
            BookingId = bookingId;
        }
    }
}