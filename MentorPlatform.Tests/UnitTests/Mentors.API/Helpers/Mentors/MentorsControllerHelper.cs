using MentorDto = Mentors.ApplicationCore.DTO.MentorDto;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Helpers.Mentors
{
    public class MentorsControllerHelper
    {
        private readonly Mock<IMentorService> _mockMentorService;

        public MentorsControllerHelper(
            Mock<IMentorService> mockMentorService)
        {
            _mockMentorService = mockMentorService;
        }

        public void SetupGetAllAsync(List<MentorDto> mentors)
        {
            _mockMentorService
                .Setup(service => service.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentors);
        }

        public void SetupGetByIdAsync(MentorDto mentor)
        {
            _mockMentorService
                .Setup(service => service.GetByIdAsync(
                    mentor.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);
        }

        public void SetupCreateAsync(MentorCreateDto mentor)
        {
            _mockMentorService
                .Setup(service => service.CreateAsync(
                    mentor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);
        }

        public void SetupUpdateAsync(MentorDto mentor)
        {
            _mockMentorService
                .Setup(service => service.UpdateAsync(
                    mentor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);
        }

        public void SetupDeleteAsync(MentorDto mentor)
        {
            _mockMentorService
                .Setup(service => service.DeleteAsync(
                    mentor.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);
        }
    }
}