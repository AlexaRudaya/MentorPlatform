using AvailabilityDto = Booking.ApplicationCore.DTO.AvailabilityDto;

namespace MentorPlatform.Tests.UnitTests.Booking.API.Services
{
    public class BookingForMentorServiceTests
    {
        private readonly Mock<IMentorBookingRepository> _mockBookingRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<BookingForMentorService>> _mockLogger;
        private readonly Mock<IBackgroundJobsService> _mockBackgroundJobsService;
        private readonly BookingForMentorService _bookingForMentorService;
        private readonly BookingGenerator _bookingGenerator;
        private readonly BookingServiceHelper _bookingHelper;
        private readonly Mock<IGetMentorClient> _mockMentorClient;
        private readonly CancellationToken _cancellationToken;

        public BookingForMentorServiceTests()
        {
            _mockBookingRepository = new Mock<IMentorBookingRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BookingForMentorService>>();
            _mockBackgroundJobsService = new Mock<IBackgroundJobsService>();
            _bookingForMentorService = new BookingForMentorService(
                _mockBookingRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockBackgroundJobsService.Object);
            _bookingGenerator = new BookingGenerator();
            _mockMentorClient = new Mock<IGetMentorClient>();
            _bookingHelper = new BookingServiceHelper(
                _mockMapper,
                _mockBookingRepository,
                _mockMentorClient);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetBookingsForMentorAsync_WhenBookingsAreFound_ShouldReturnBookingDto()
        {
            // Arrange
            var mentorId = "123e4567-e89b-12d3-a456-9AC7CBDCEE52";
            var bookings = new List<MentorBooking>
            {
                _bookingGenerator.GenerateFakeBooking(),
                _bookingGenerator.GenerateFakeBooking(),
                _bookingGenerator.GenerateFakeBooking(),
            };
            var bookingsDto = _bookingHelper.GenerateDtoList(bookings);

            _bookingHelper.SetupGetBookingsForMentorAsync(bookings);
            _bookingHelper.SetupMapperForBookingsListToDto(bookings, bookingsDto);

            // Act
            var result = await _bookingForMentorService.GetBookingsForMentorAsync(mentorId, _cancellationToken);
            
            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetBookingsForMentorAsync_WhenBookingsAreNull_ShouldThrowBookingNotFoundException()
        {
            // Arrange
            var mentorId = "123e4567-e89b-12d3-a456-9AC7CBDCEE52";

            _bookingHelper.SetupGetBookingsForMentorAsync();

            // Act
            var result = async() => await _bookingForMentorService.GetBookingsForMentorAsync(mentorId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<BookingNotFoundException>();
        }

        [Fact]
        public async Task GetAvailabilitiesOfMentor_WhenAvailabilitiesAreFound_ShouldReturnListOfAvailabilities()
        {
            // Arrange
            var mentorId = "123e4567-e89b-12d3-a456-9AC7CBDCEE52";
            var availabilities = new List<AvailabilityDto>
            {
                _bookingGenerator.GenerateAvailabilityDto(),
                _bookingGenerator.GenerateAvailabilityDto()
            };

            _mockBackgroundJobsService
                .Setup(service => service.GetMentorAvailabilitiesFromMentorApi(
                    mentorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availabilities);

            // Act
            var result = await _bookingForMentorService.GetAvailabilitiesOfMentor(mentorId, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(2);
        }
    }
}