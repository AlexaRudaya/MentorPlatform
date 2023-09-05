namespace MentorPlatform.Tests.UnitTests.Mentors.API.Controllers
{
    public class AvailabilitiesControllerTests
    {
        private readonly Mock<IAvailabilityService> _mockAvailabilityService;
        private readonly AvailabilitiesController _controller;
        private readonly AvailabilityGenerator _availabilityData;
        private readonly AvailabilitiesControllerHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public AvailabilitiesControllerTests()
        {
            _mockAvailabilityService = new Mock<IAvailabilityService>();
            _controller = new AvailabilitiesController(_mockAvailabilityService.Object);
            _availabilityData = new AvailabilityGenerator();
            _helper = new AvailabilitiesControllerHelper(_mockAvailabilityService);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetAvailabilities_ShouldReturnOkWithAvailabilities()
        {
            // Arrange
            var availabilities = new List<AvailabilityDto>
            {
                _availabilityData.GenerateFakeDto(),
                _availabilityData.GenerateFakeDto(),
                _availabilityData.GenerateFakeDto()
            };

            _helper.SetupGetAllAsync(availabilities);

            // Act
            var result = await _controller.GetAvailabilities(_cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedAvailabilities = okResult.Value
                .Should().BeOfType<List<AvailabilityDto>>();
        }

        [Fact]
        public async Task GetAvailability_ShouldReturnOkWithAvailability()
        {
            // Arrange
            var availability = _availabilityData.GenerateFakeDto();

            _helper.SetupGetByIdAsync(availability);

            // Act
            var result = await _controller.GetAvailability(availability.Id, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedAvailability = okResult.Value
                .Should().BeOfType<AvailabilityDto>();
        }

        [Fact]
        public async Task CreateAvailability_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var availability = _availabilityData.GenerateFakeDto();

            _helper.SetupCreateAsync(availability);

            // Act
            var result = await _controller.CreateAvailability(availability, _cancellationToken);

            // Assert
            var createdAtResult = result
                .Should().BeOfType<CreatedAtActionResult>().Subject;

            var returnedAvailability = createdAtResult.Value
                .Should().BeOfType<AvailabilityDto>();
        }

        [Fact]
        public async Task UpdateAvailability_ShouldReturnNoContent()
        {
            // Arrange
            var availability = _availabilityData.GenerateFakeDto();

            _helper.SetupUpdateAsync(availability);

            // Act
            var result = await _controller.UpdateAvailability(availability, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAvailability_ShouldReturnNoContent()
        {
            // Arrange
            var availability = _availabilityData.GenerateFakeDto();

            _helper.SetupUpdateAsync(availability);

            // Act
            var result = await _controller.DeleteAvailability(availability.Id, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}