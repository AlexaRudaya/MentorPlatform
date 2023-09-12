using AvailabilityDto = Mentors.ApplicationCore.DTO.AvailabilityDto;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Helpers.Availabilities
{
    public class AvailabilitiesControllerHelper
    {
        private readonly Mock<IAvailabilityService> _mockAvailabilityService;

        public AvailabilitiesControllerHelper(
            Mock<IAvailabilityService> mockAvailabilityService)
        {
            _mockAvailabilityService = mockAvailabilityService;
        }

        public void SetupGetAllAsync(List<AvailabilityDto> availabilities)
        {
            _mockAvailabilityService
                .Setup(service => service.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(availabilities);
        }

        public void SetupGetByIdAsync(AvailabilityDto availability)
        {
            _mockAvailabilityService
                .Setup(service => service.GetByIdAsync(
                    availability.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);
        }

        public void SetupCreateAsync(AvailabilityDto availability)
        {
            _mockAvailabilityService
                .Setup(service => service.CreateAsync(
                    availability, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);
        }

        public void SetupUpdateAsync(AvailabilityDto availability)
        {
            _mockAvailabilityService
                .Setup(service => service.UpdateAsync(
                    availability, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);
        }

        public void SetupDeleteAsync(AvailabilityDto availability)
        {
            _mockAvailabilityService
                .Setup(service => service.DeleteAsync(
                    availability.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);
        }
    }
}