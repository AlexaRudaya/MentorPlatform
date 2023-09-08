using Booking.API.Controllers;
using Booking.ApplicationCore.DTO;
using Booking.ApplicationCore.Interfaces.IService;
using MentorPlatform.Tests.UnitTests.Booking.API.BogusData;
using MentorPlatform.Tests.UnitTests.Booking.API.Helpers.Bookings;
using Moq;

namespace MentorPlatform.Tests.UnitTests.Booking.API.Controllers
{
    public class BookingsControllerTests
    {
        private readonly Mock<IBookingService> _mockBookingService;
        private readonly Mock<IBookingForMentorService> _mockBookingForMentorService;
        private readonly BookingsController _controller;
        private readonly BookingGenerator _bookingGenerator;
        private readonly BookingsControllerHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public BookingsControllerTests()
        {
            _mockBookingService = new Mock<IBookingService>();
            _mockBookingForMentorService = new Mock<IBookingForMentorService>();
            _controller = new BookingsController(
                _mockBookingService.Object,
                _mockBookingForMentorService.Object);
            _bookingGenerator = new BookingGenerator();
            _helper = new BookingsControllerHelper(
                _mockBookingService,
                _mockBookingForMentorService);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetBookings_WhenModelsAreFound_ShouldReturnOkWithBookings()
        {
            // Arrange
            var bookings = new List<BookingDto>
            {
                _bookingGenerator.GenerateFakeBookingDto(),
                _bookingGenerator.GenerateFakeBookingDto(),
                _bookingGenerator.GenerateFakeBookingDto(),
            };

            _helper.SetupGetAllAsync(bookings);

            // Act
            var result = await _controller.GetBookings(_cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedCategories = okResult.Value
                .Should().BeOfType<List<BookingDto>>();
        }

        [Fact]
        public async Task GetBooking_WhenModelIsFound_ShouldReturnOkWithBooking()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBookingDto();

            _helper.SetupGetByIdAsync(booking);

            // Act
            var result = await _controller.GetBooking(booking.Id, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedCategory = okResult.Value
                .Should().BeOfType<BookingDto>();
        }

        [Fact]
        public async Task GetBookingsForStudent_WhenModelIsFound_ShouldReturnOkWithBookingsForStudent()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var bookings = new List<BookingDto>
            {
                _bookingGenerator.GenerateFakeBookingDto(),
                _bookingGenerator.GenerateFakeBookingDto(),
            };

            _helper.SetupGetBookingsForStudent(studentId, bookings);

            // Act
            var result = await _controller.GetBookingsForStudent(studentId, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedBookings = okResult.Value
                .Should().BeOfType<List<BookingDto>>();
        }

        [Fact]
        public async Task GetBookingsForMentor_WhenModelIsFound_ShouldReturnOkWithBookingsForMentor()
        {
            // Arrange
            var mentorId = "546c776b3e23f5f2ebdd3b03";
            var bookings = new List<BookingDto>
            {
                _bookingGenerator.GenerateFakeBookingDto(),
                _bookingGenerator.GenerateFakeBookingDto(),
            };
            
            _helper.SetupGetBookingsForMentor(mentorId, bookings);

            // Act
            var result = await _controller.GetBookingsForMentor(mentorId, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedBookings = okResult.Value
                .Should().BeOfType<List<BookingDto>>();
        }

        [Fact]
        public async Task GetAvailabilitiesOfMentor_WhenModelIFound_ShouldReturnOkWithAvailabilitiesOfMentor()
        {
            // Arrange
            var mentorId = "546c776b3e23f5f2ebdd3b03";
            var availabilities = new List<AvailabilityDto>
            {
                _bookingGenerator.GenerateAvailabilityDto(),
                _bookingGenerator.GenerateAvailabilityDto()
            };

            _helper.SetupGetAvailabilitiesOfMentor(mentorId, availabilities);

            // Act
            var result = await _controller.GetAvailabilitiesOfMentor(mentorId, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedBookings = okResult.Value
                .Should().BeOfType<List<AvailabilityDto>>();
        }

        [Fact]
        public async Task CreateBooking_WhenModelIsValid_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBookingDto();

            _helper.SetupCreateAsync(booking);

            // Act
            var result = await _controller.CreateBooking(booking, _cancellationToken);

            // Assert
            var createdAtResult = result
                .Should().BeOfType<CreatedAtActionResult>().Subject;

            var returnedCategory = createdAtResult.Value
                .Should().BeOfType<BookingDto>();
        }

        [Fact]
        public async Task UpdateBooking_WhenModelIsFound_ShouldReturnNoContent()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBookingDto();

            _helper.SetupUpdateAsync(booking);

            // Act
            var result = await _controller.UpdateBooking(booking, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteBooking_WhenModelIsFound_ShouldReturnNoContent()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBookingDto();

            _helper.SetupDeleteAsync(booking);

            // Act
            var result = await _controller.DeleteBooking(booking.Id, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}