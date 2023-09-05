namespace MentorPlatform.Tests.UnitTests.Mentors.API.Services
{
    public class AvailabilityServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAvailabilityRepository> _mockAvailabilityRepository;
        private readonly Mock<ILogger<AvailabilityService>> _mockLogger;
        private readonly Mock<IProducer> _mockProducer;
        private readonly IAvailabilityService _availabilityService;
        private readonly AvailabilityGenerator _availabilityGenerator;
        private readonly AvailabilityServiceHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public AvailabilityServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockAvailabilityRepository = new Mock<IAvailabilityRepository>();
            _mockLogger = new Mock<ILogger<AvailabilityService>>();
            _mockProducer = new Mock<IProducer>();
            _availabilityGenerator = new AvailabilityGenerator();
            _availabilityService = new AvailabilityService(
                _mockAvailabilityRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockProducer.Object);
            _helper = new AvailabilityServiceHelper(_mockAvailabilityRepository,
                _mockProducer,
                _mockMapper);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfAvailabilityDto()
        {
            // Arrange
            var availabilities = new List<Availability>
            {
                _availabilityGenerator.GenerateFakeAvailability(),
                _availabilityGenerator.GenerateFakeAvailability(),
                _availabilityGenerator.GenerateFakeAvailability(),
            };

            _helper.SetupGetAllAsync(availabilities);

            var availabilitiesDto = _helper.GenerateDtoList(availabilities);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<AvailabilityDto>>(availabilities))
                .Returns(availabilitiesDto);

            // Act
            var result = await _availabilityService.GetAllAsync(_cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenAvailabilitiesAreNull_ShouldThrowAvailabilityNotFoundException()
        {
            // Arrange
            _helper.SetupGetAllAsyncWhenNull();

            // Act
            var result = async() => await _availabilityService.GetAllAsync(_cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<AvailabilityNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAvailabilityDto()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();

            _helper.SetupGetByIdAsync(availability);

            var availabilityDto = _helper.GenerateDtoFromAvailability(availability);

            _helper.SetupMapperForAvailabilityToDto(availability, availabilityDto);

            // Act
            var result = await _availabilityService.GetByIdAsync(availability.Id, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result.Id
                .Should().Be(availability.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAvailabilityIsNull_ShouldThrowAvailabilityNotFoundException()
        {
            // Arrange
            var availabilityId = _availabilityGenerator.GenerateFakeAvailability().Id;

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _availabilityService.GetByIdAsync(availabilityId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<AvailabilityNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedAvailabilityDto()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();

            var availabilityDto = _helper.GenerateDtoFromAvailability(availability);

            _helper.SetupMapperForDtoToAvailability(availability, availabilityDto);

            _helper.SetupCreateAsync(availability);

            _helper.SetupMapperForAvailabilityToDto(availability, availabilityDto);

            // Act
            var result = await _availabilityService.CreateAsync(availabilityDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(availabilityDto.Id);

            _mockAvailabilityRepository
                .Verify(repository => repository
                    .CreateAsync(availability, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldPubishAvailabilityEvent()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();

            var availabilityDto = _helper.GenerateDtoFromAvailability(availability);

            var eventToPublish = new AvailabilityOfMentorEvent();

            _helper.SetupMapperForDtoToAvailability(availability, availabilityDto);

            _helper.SetupMapperForAvailabilityToDto(availability, availabilityDto);

            _mockMapper
                .Setup(mapper => mapper.Map<AvailabilityOfMentorEvent>(availability))
                .Returns(eventToPublish);

            _helper.SetupPublishAvailabilityEvent(eventToPublish);

            // Act
            var result = await _availabilityService.CreateAsync(availabilityDto);

            // Assert
            result.Should().NotBeNull();

            _mockProducer
                .Verify(producer => producer
                    .PublishAsync(eventToPublish, _cancellationToken),
                Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedAvailability()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();

            var availabilityDto = _helper.GenerateDtoFromAvailability(availability);

            _helper.SetupGetByIdAsync(availability);

            _helper.SetupMapperForDtoToAvailability(availability, availabilityDto);

            // Act
            var result = await _availabilityService.UpdateAsync(availabilityDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockAvailabilityRepository
                .Verify(repository => repository
                    .UpdateAsync(availability, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenAvailabilityIsNull_ShouldThrowAvailabilityNotFoundException()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();

            var availabilityDto = _helper.GenerateDtoFromAvailability(availability);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _availabilityService.UpdateAsync(availabilityDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<AvailabilityNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAvailability()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();

            var availabilityDto = _helper.GenerateDtoFromAvailability(availability);

            _helper.SetupDeleteAsync(availability);

            _helper.SetupMapperForAvailabilityToDto(availability, availabilityDto);

            // Act
            var result = await _availabilityService.DeleteAsync(availability.Id);

            // Assert
            result
                .Should().NotBeNull();

            _mockAvailabilityRepository
                .Verify(repository => repository
                    .DeleteAsync(availability, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenAvailabilityIsNull_ShouldThrowAvailabilityNotFoundException()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();

            var availabilityDto = _helper.GenerateDtoFromAvailability(availability);

            _helper.SetupGetAllAsyncWhenNull();

            // Act
            var result = async() => await _availabilityService.DeleteAsync(availability.Id, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<AvailabilityNotFoundException>();
        }
    }
}