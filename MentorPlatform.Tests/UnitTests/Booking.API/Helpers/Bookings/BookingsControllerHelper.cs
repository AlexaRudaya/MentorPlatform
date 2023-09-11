namespace MentorPlatform.Tests.UnitTests.Booking.API.Helpers.Bookings
{
    public class BookingsControllerHelper
    {
        private readonly Mock<IBookingService> _mockBookingService;
        private readonly Mock<IBookingForMentorService> _mockBookingForMentorService;

        public BookingsControllerHelper(
            Mock<IBookingService> mockBookingService,
            Mock<IBookingForMentorService> mockBookingForMentorService)
        {
            _mockBookingService = mockBookingService;
            _mockBookingForMentorService = mockBookingForMentorService;
        }

        public void SetupGetAllAsync(List<BookingDto> bookings)
        {
            _mockBookingService
                .Setup(service => service.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);
        }

        public void SetupGetByIdAsync(BookingDto booking)
        {
            _mockBookingService
                .Setup(service => service.GetByIdAsync(
                    booking.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
        }

        public void SetupGetBookingsForStudent(Guid studentId,
            List<BookingDto> bookings)
        {
            _mockBookingService
                .Setup(service => service.GetBookingsForStudentAsync(
                    studentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);
        }

        public void SetupGetBookingsForMentor(string mentorId,
            List<BookingDto> bookings)
        {
            _mockBookingForMentorService
                .Setup(service => service.GetBookingsForMentorAsync(
                    mentorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);
        }

        public void SetupGetAvailabilitiesOfMentor(string mentorId,
            List<AvailabilityDto> availabilities)
        {
            _mockBookingForMentorService
              .Setup(service => service.GetAvailabilitiesOfMentor(
                  mentorId,
                  It.IsAny<CancellationToken>()))
              .ReturnsAsync(availabilities);
        }

        public void SetupCreateAsync(BookingDto booking)
        {
            _mockBookingService
                .Setup(service => service.CreateAsync(
                    booking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
        }

        public void SetupUpdateAsync(BookingDto booking)
        {
            _mockBookingService
                .Setup(service => service.UpdateAsync(
                    booking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
        }

        public void SetupDeleteAsync(BookingDto booking)
        {
            _mockBookingService
                .Setup(service => service.DeleteAsync(
                    booking.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
        }
    }
}