namespace MentorPlatform.Tests.UnitTests.Mentors.API.Controllers
{
    public class MentorshipSubjectsControllerTests
    {
        private readonly Mock<IMentorshipSubjectService> _mockSubjectService;
        private readonly MentorshipSubjectsController _controller;
        private readonly MentorshipSubjectGenerator _subjectData;
        private readonly SubjectsControllerHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public MentorshipSubjectsControllerTests()
        {
            _mockSubjectService = new Mock<IMentorshipSubjectService>();
            _controller = new MentorshipSubjectsController(_mockSubjectService.Object);
            _subjectData = new MentorshipSubjectGenerator();
            _helper = new SubjectsControllerHelper(_mockSubjectService);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetSubjects_ShouldReturnOkWithSubjects()
        {
            // Arrange
            var subjects = new List<MentorshipSubjectDto>
            {
                _subjectData.GenerateFakeDto(),
                _subjectData.GenerateFakeDto(),
                _subjectData.GenerateFakeDto(),
            };

            _helper.SetupGetAllAsync(subjects);

            // Act
            var result = await _controller.GetSubjects(_cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedSubjects = okResult.Value
                .Should().BeOfType<List<MentorshipSubjectDto>>();
        }

        [Fact]
        public async Task GetSubject_ShouldReturnOkWithSubject()
        {
            // Arrange
            var subject = _subjectData.GenerateFakeDto();

            _helper.SetupGetByIdAsync(subject);

            // Act
            var result = await _controller.GetSubject(subject.Id, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedSubject = okResult.Value
                .Should().BeOfType<MentorshipSubjectDto>();
        }

        [Fact]
        public async Task CreateSubject_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var subject = _subjectData.GenerateFakeDto();

            _helper.SetupCreateAsync(subject);

            // Act
            var result = await _controller.CreateSubject(subject, _cancellationToken);

            // Assert
            var createdAtResult = result
                .Should().BeOfType<CreatedAtActionResult>().Subject;

            var returnedSubject = createdAtResult.Value
                .Should().BeOfType<MentorshipSubjectDto>();
        }

        [Fact]
        public async Task UpdateSubject_ShouldReturnNoContent()
        {
            // Arrange
            var subject = _subjectData.GenerateFakeDto();

            _helper.SetupUpdateAsync(subject);

            // Act
            var result = await _controller.UpdateSubject(subject, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteSubject_ShouldReturnNoContent()
        {
            // Arrange
            var subject = _subjectData.GenerateFakeDto();

            _helper.SetupDeleteAsync(subject);

            // Act
            var result = await _controller.DeleteSubject(subject.Id, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}