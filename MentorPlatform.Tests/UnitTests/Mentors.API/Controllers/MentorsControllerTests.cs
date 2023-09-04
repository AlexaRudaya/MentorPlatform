using MentorPlatform.Tests.UnitTests.Mentors.API.BogusData;
using Mentors.API.Controllers;
using Mentors.ApplicationCore.DTO;
using Mentors.ApplicationCore.Interfaces.IService;
using Moq;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Controllers
{
    public class MentorsControllerTests
    {
        private readonly Mock<IMentorService> _mockMentorService;
        private readonly MentorsController _controller;
        private readonly MentorGenerator _mentorData;

        public MentorsControllerTests()
        {
            _mockMentorService = new Mock<IMentorService>();
            _controller = new MentorsController(_mockMentorService.Object);

            var categoryGenerator = new CategoryGenerator();
            var availabilityGenerator = new AvailabilityGenerator();
            _mentorData = new MentorGenerator(categoryGenerator, availabilityGenerator);
        }

        [Fact]
        public async Task GetMentors_ShouldReturnOkWithMentors()
        {
            // Arrange
            var mentors = new List<MentorDto>
            {
                _mentorData.GenerateFakeMentorDto(),
                _mentorData.GenerateFakeMentorDto(),
                _mentorData.GenerateFakeMentorDto(),
            };
            var cancellationToken = CancellationToken.None;

            _mockMentorService
                .Setup(service => service
                .GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentors);

            // Act
            var result = await _controller.GetMentors(cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedMentors = okResult.Value
                .Should().BeOfType<List<MentorDto>>();
        }

        [Fact]
        public async Task GetMentor_ShouldReturnOkWithMentor()
        {
            // Arrange
            var mentor = _mentorData.GenerateFakeMentorDto();
            var cancellationToken = CancellationToken.None;

            _mockMentorService
                .Setup(service => service
                .GetByIdAsync(mentor.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);

            // Act
            var result = await _controller.GetMentor(mentor.Id, cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedMentor = okResult.Value
                .Should().BeOfType<MentorDto>();
        }

        [Fact]
        public async Task CreateMentor_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var mentor = _mentorData.GenerateFakeDto();
            var cancellationToken = CancellationToken.None;

            _mockMentorService
                .Setup(service => service.CreateAsync(
                    mentor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);

            // Act
            var result = await _controller.CreateMentor(mentor, cancellationToken);

            // Assert
            var createdAtResult = result
                .Should().BeOfType<CreatedAtActionResult>().Subject;

            var returnedMentor = createdAtResult.Value
                .Should().BeOfType<MentorCreateDto>();
        }

        [Fact]
        public async Task UpdateMentor_ShouldReturnNoContent()
        {
            // Arrange
            var mentor = _mentorData.GenerateFakeMentorDto();
            var cancellationToken = CancellationToken.None;

            _mockMentorService
                .Setup(service => service.UpdateAsync(
                    mentor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);

            // Act
            var result = await _controller.UpdateMentor(mentor, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteMentor_ShouldReturnNoContent()
        {
            // Arrange
            var mentor = _mentorData.GenerateFakeMentorDto();
            var cancellationToken = CancellationToken.None;

            _mockMentorService
                .Setup(service => service.DeleteAsync(
                    mentor.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);

            // Act
            var result = await _controller.DeleteMentor(mentor.Id, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}