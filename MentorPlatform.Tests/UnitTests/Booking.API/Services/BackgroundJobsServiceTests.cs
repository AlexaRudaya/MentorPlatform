using AvailabilityDto = Booking.ApplicationCore.DTO.AvailabilityDto;
using MentorDto = Booking.ApplicationCore.DTO.MentorDto;
using ObjectNotFoundException = Booking.ApplicationCore.Exceptions.ObjectNotFoundException;

namespace MentorPlatform.Tests.UnitTests.Booking.API.Services
{
    public class BackgroundJobsServiceTests
    {
        private readonly Mock<IProducer> _mockProducer;
        private readonly Mock<IGetMentorClient> _mockMentorClient;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<BackgroundJobsService>> _mockLogger;
        private readonly BackgroundJobsService _backgroundJobsService;
        private readonly Mock<IMentorBookingRepository> _mockBookingRepository;
        private readonly BookingGenerator _bookingGenerator;
        private readonly BookingServiceHelper _bookingHelper;
        private readonly CancellationToken _cancellationToken;

        public BackgroundJobsServiceTests()
        {
            _mockProducer = new Mock<IProducer>();
            _mockMentorClient = new Mock<IGetMentorClient>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BackgroundJobsService>>();
            _backgroundJobsService = new BackgroundJobsService(
                _mockProducer.Object,
                _mockMentorClient.Object,
                _mockMapper.Object,
                _mockLogger.Object);
            _mockBookingRepository = new Mock<IMentorBookingRepository>();
            _bookingGenerator = new BookingGenerator();
            _bookingHelper = new BookingServiceHelper(_mockMapper,
                _mockBookingRepository,
                _mockMentorClient);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task PublishBookingEvent_WhenCalled_ShouldPrublishEvent()
        {
            // Arrange
            var bookingEventToPublish = new MeetingBookingEvent();

            _mockProducer
                .Setup(producer => producer.PublishAsync(bookingEventToPublish, _cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _backgroundJobsService.PublishBookingEvent(bookingEventToPublish, _cancellationToken);

            // Assert
            _mockProducer
                .Verify(producer => producer.PublishAsync(
                    bookingEventToPublish, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task GetMentorAvailabilitiesFromMentorApi_WhenMentorIsFound_ShouldReturnListOfAvailabilities()
        {
            // Arrange
            var mentorId = "123e4567-e89b-12d3-a456-9AC7CBDCEE52";
            var mentorReply = new MentorDto();
            var availabilities = new List<AvailabilityDto>
            {
                _bookingGenerator.GenerateAvailabilityDto(),
                _bookingGenerator.GenerateAvailabilityDto()
            };

            _mockMentorClient
                .Setup(client => client.GetMentorAsync(
                   mentorId,
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(mentorReply);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<AvailabilityDto>>(mentorReply.Availabilities))
                .Returns(availabilities);

            // Act
            var result = await _backgroundJobsService.GetMentorAvailabilitiesFromMentorApi(mentorId, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(2);
        }

        [Fact]
        public async Task GetMentorAvailabilitiesFromMentorApi_WhenMentorIsNull_ShouldThrowObjectNotFoundException()
        {
            // Arrange
            var mentorId = "123e4567-e89b-12d3-a456-9AC7CBDCEE52";
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _bookingHelper.GenerateDtoFromBooking(booking);

            _bookingHelper.SetupGetMentorAsyncWhenNull(bookingDto);

            // Act
            var result = async() => await _backgroundJobsService.GetMentorAvailabilitiesFromMentorApi(mentorId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<ObjectNotFoundException>();
        }
    }
}