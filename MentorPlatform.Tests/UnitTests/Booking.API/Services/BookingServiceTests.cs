using MentorDto = Booking.ApplicationCore.DTO.MentorDto;
using ObjectNotFoundException = Booking.ApplicationCore.Exceptions.ObjectNotFoundException;

namespace MentorPlatform.Tests.UnitTests.Booking.API.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMentorBookingRepository> _mockBookingRepository;
        private readonly Mock<IStudentRepository> _mockStudentRepository;
        private readonly Mock<IGetMentorClient> _mockMentorClient;
        private readonly Mock<ILogger<BookingService>> _mockLogger;
        private readonly Mock<IBackgroundJobsService> _mockBackgroundJobsService;
        private readonly IBookingService _bookingService;
        private readonly BookingGenerator _bookingGenerator;
        private readonly StudentGenerator _studentGenerator;
        private readonly BookingServiceHelper _helper;
        private readonly StudentServiceHelper _studentHelper;
        private readonly CancellationToken _cancellationToken;

        public BookingServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockBookingRepository = new Mock<IMentorBookingRepository>();
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockMentorClient = new Mock<IGetMentorClient>();
            _mockLogger = new Mock<ILogger<BookingService>>();
            _mockBackgroundJobsService = new Mock<IBackgroundJobsService>();
            _bookingService = new BookingService(
                _mockBookingRepository.Object,
                _mockStudentRepository.Object,
                _mockMentorClient.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockBackgroundJobsService.Object);
            _bookingGenerator = new BookingGenerator();
            _studentGenerator = new StudentGenerator();
            _helper = new BookingServiceHelper(_mockMapper,
                _mockBookingRepository,
                _mockMentorClient);
            _studentHelper = new StudentServiceHelper(_mockMapper,
                _mockStudentRepository);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetAllAsync_WhenBookingsAreFound_ShouldReturnListOfBookingsDto()
        {
            // Arrange
            var bookings = new List<MentorBooking>
            {
                _bookingGenerator.GenerateFakeBooking(),
                _bookingGenerator.GenerateFakeBooking(),
                _bookingGenerator.GenerateFakeBooking(),
            };
            var bookingsDto = _helper.GenerateDtoList(bookings);

            _helper.SetupGetAllAsync(bookings);
            _helper.SetupMapperForBookingsListToDto(bookings, bookingsDto);

            // Act
            var result = await _bookingService.GetAllAsync(_cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenBookingsAreNull_ShouldThrowBookingNotFoundException()
        {
            // Arrange
            _helper.SetupGetAllAsyncWhenNull();

            // Act
            var result = async() => await _bookingService.GetAllAsync(_cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<BookingNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_WhenBookingIsFound_ShouldReturnBookingDto()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);

            _helper.SetupGetByIdAsync(booking);
            _helper.SetupMapperForBookingToDto(booking, bookingDto);

            // Act
            var result = await _bookingService.GetByIdAsync(booking.Id, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result.Id
                .Should().Be(booking.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenBookingIsNull_ShouldThrowBookingNotFoundException()
        {
            // Arrange
            var bookingId = _bookingGenerator.GenerateFakeBooking().Id;

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _bookingService.GetByIdAsync(bookingId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<BookingNotFoundException>();
        }

        [Fact]
        public async Task GetBookingsForStudentAsync_WhenBookingsAreFound_ShouldReturnBookingDto()
        {
            // Arrange
            var bookings = new List<MentorBooking>
            {
                _bookingGenerator.GenerateFakeBooking(),
                _bookingGenerator.GenerateFakeBooking(),
                _bookingGenerator.GenerateFakeBooking(),
            };
            var bookingsDto = _helper.GenerateDtoList(bookings);
            var studentId = Guid.NewGuid();

            _helper.SetupGetBookingsForStudent(studentId, bookings);
            _helper.SetupMapperForBookingsListToDto(bookings, bookingsDto);

            // Act
            var result = await _bookingService.GetBookingsForStudentAsync(studentId, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetBookingsForStudentAsync_WhenBookingsAreNull_ShouldReturnBookingNotFoundException()
        {
            // Arrange
            var studentId = Guid.NewGuid();

            _helper.SetupGetBookingsForStudentWhenNull(studentId);

            // Act
            var result = async() => await _bookingService.GetBookingsForStudentAsync(studentId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<BookingNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsValid_ShouldReturnCreatedBookingDto()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);
            var student = _studentGenerator.GenerateFakeStudent();
            var mentorReply = new MentorDto();

            _studentHelper.SetupGetByIdAsync(student);
            _helper.SetupMapperForDtoToBooking(booking, bookingDto);
            _helper.SetupGetMentorAsync(mentorReply, bookingDto);
            _helper.SetupCreateAsync(booking);

            // Act
            var result = await _bookingService.CreateAsync(bookingDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(bookingDto.Id);

            _mockBookingRepository
                .Verify(repository => repository
                    .CreateAsync(booking, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenStudentIsNull_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);

            _helper.SetupMapperForDtoToBooking(booking, bookingDto);
            _studentHelper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _bookingService.CreateAsync(bookingDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<StudentNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_WhenMentorIsNull_ShouldThrowObjectNotFoundException()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);
            var student = _studentGenerator.GenerateFakeStudent();

            _helper.SetupMapperForDtoToBooking(booking, bookingDto);
            _studentHelper.SetupGetByIdAsync(student);
            _helper.SetupGetMentorAsyncWhenNull(bookingDto);

            // Act
            var result = async() => await _bookingService.CreateAsync(bookingDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<ObjectNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsValid_ShouldPublishUsingBackgroundJobs()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);
            var student = _studentGenerator.GenerateFakeStudent();
            var mentorReply = new MentorDto();
            var eventToPublish = new MeetingBookingEvent();

            _helper.SetupMapperForDtoToBooking(booking, bookingDto);
            _studentHelper.SetupGetByIdAsync(student);
            _helper.SetupGetMentorAsync(mentorReply, bookingDto);
            _helper.SetupCreateAsync(booking);
            _helper.SetupMapperForDtoToBookingEvent(booking, eventToPublish);

            _mockBackgroundJobsService
                .Setup(service => service.PublishBookingEvent(
                    It.IsAny<MeetingBookingEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _bookingService.CreateAsync(bookingDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            _mockBackgroundJobsService.Verify();
        }

        [Fact]
        public async Task UpdateAsync_WhenBookingIsFound_ShouldReturnUpdatedBookingDto()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);

            _helper.SetupGetByIdAsync(booking);
            _helper.SetupMapperForDtoToBooking(booking, bookingDto);

            // Act
            var result = await _bookingService.UpdateAsync(bookingDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockBookingRepository
                .Verify(repository => repository
                    .UpdateAsync(booking, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenBookingIsNull_ShouldThrowBookingNotFoundException()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _bookingService.UpdateAsync(bookingDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<BookingNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenBookingIsFound_ShouldDeleteBooking()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);

            _helper.SetupGetByIdAsync(booking);
            _helper.SetupMapperForBookingToDto(booking, bookingDto);

            // Act
            var result = await _bookingService.DeleteAsync(booking.Id);

            // Assert
            result
                .Should().NotBeNull();

            _mockBookingRepository
                .Verify(repository => repository
                    .DeleteAsync(booking, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenBookingIsNull_ShouldThrowBookingNotFoundException()
        {
            // Arrange
            var booking = _bookingGenerator.GenerateFakeBooking();
            var bookingDto = _helper.GenerateDtoFromBooking(booking);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _bookingService.DeleteAsync(booking.Id, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<BookingNotFoundException>();
        }
    }
}