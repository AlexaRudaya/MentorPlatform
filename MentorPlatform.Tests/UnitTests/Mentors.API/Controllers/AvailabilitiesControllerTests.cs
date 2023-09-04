using MentorPlatform.Tests.UnitTests.Mentors.API.BogusData;
using Mentors.API.Controllers;
using Mentors.ApplicationCore.DTO;
using Mentors.ApplicationCore.Interfaces.IService;
using Moq;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Controllers
{
    public class AvailabilitiesControllerTests
    {
        private readonly Mock<IAvailabilityService> _mockAvailabilityService;
        private readonly AvailabilitiesController _controller;
        private readonly AvailabilityGenerator _availabilityData;

        public AvailabilitiesControllerTests()
        {
            _mockAvailabilityService = new Mock<IAvailabilityService>();
            _controller = new AvailabilitiesController(_mockAvailabilityService.Object);
            _availabilityData = new AvailabilityGenerator();
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
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityService
                .Setup(service => service
                .GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(availabilities);

            // Act
            var result = await _controller.GetAvailabilities(cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityService
                .Setup(service => service
                .GetByIdAsync(availability.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);

            // Act
            var result = await _controller.GetAvailability(availability.Id, cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityService
               .Setup(service => service.CreateAsync(
                    availability, It.IsAny<CancellationToken>()))
               .ReturnsAsync(availability);

            // Act
            var result = await _controller.CreateAvailability(availability, cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityService
                .Setup(service => service.UpdateAsync(
                    availability, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);

            // Act
            var result = await _controller.UpdateAvailability(availability, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAvailability_ShouldReturnNoContent()
        {
            // Arrange
            var availability = _availabilityData.GenerateFakeDto();
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityService
                .Setup(service => service.DeleteAsync(
                    availability.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);

            // Act
            var result = await _controller.DeleteAvailability(availability.Id, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}