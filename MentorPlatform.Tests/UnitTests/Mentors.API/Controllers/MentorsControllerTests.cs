using MentorDto = Mentors.ApplicationCore.DTO.MentorDto;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Controllers
{
    public class MentorsControllerTests
    {
        private readonly Mock<IMentorService> _mockMentorService;
        private readonly MentorsController _controller;
        private readonly MentorGenerator _mentorData;
        private readonly MentorsControllerHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public MentorsControllerTests()
        {
            _mockMentorService = new Mock<IMentorService>();
            _controller = new MentorsController(_mockMentorService.Object);
            var categoryGenerator = new CategoryGenerator();
            var availabilityGenerator = new AvailabilityGenerator();
            _mentorData = new MentorGenerator(categoryGenerator, availabilityGenerator);
            _helper = new MentorsControllerHelper(_mockMentorService);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetMentors_WhenModelsAreFound_ShouldReturnOkWithMentors()
        {
            // Arrange
            var mentors = new List<MentorDto>
            {
                _mentorData.GenerateFakeMentorDto(),
                _mentorData.GenerateFakeMentorDto(),
                _mentorData.GenerateFakeMentorDto(),
            };
            _helper.SetupGetAllAsync(mentors);

            // Act
            var result = await _controller.GetMentors(_cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedMentors = okResult.Value
                .Should().BeOfType<List<MentorDto>>();
        }

        [Fact]
        public async Task GetMentor_WhenModelIsFound_ShouldReturnOkWithMentor()
        {
            // Arrange
            var mentor = _mentorData.GenerateFakeMentorDto();
            _helper.SetupGetByIdAsync(mentor);

            // Act
            var result = await _controller.GetMentor(mentor.Id, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedMentor = okResult.Value
                .Should().BeOfType<MentorDto>();
        }

        [Fact]
        public async Task CreateMentor_WhenModelIsValid_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var mentor = _mentorData.GenerateFakeDto();
            _helper.SetupCreateAsync(mentor);

            // Act
            var result = await _controller.CreateMentor(mentor, _cancellationToken);

            // Assert
            var createdAtResult = result
                .Should().BeOfType<CreatedAtActionResult>().Subject;

            var returnedMentor = createdAtResult.Value
                .Should().BeOfType<MentorCreateDto>();
        }

        [Fact]
        public async Task UpdateMentor_WhenModelIsFound_ShouldReturnNoContent()
        {
            // Arrange
            var mentor = _mentorData.GenerateFakeMentorDto();
            _helper.SetupUpdateAsync(mentor);

            // Act
            var result = await _controller.UpdateMentor(mentor, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteMentor_WhenModelIsFound_ShouldReturnNoContent()
        {
            // Arrange
            var mentor = _mentorData.GenerateFakeMentorDto();
            _helper.SetupDeleteAsync(mentor);

            // Act
            var result = await _controller.DeleteMentor(mentor.Id, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}