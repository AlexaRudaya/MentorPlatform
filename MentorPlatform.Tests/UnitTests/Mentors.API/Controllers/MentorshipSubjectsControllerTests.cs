using MentorPlatform.Tests.UnitTests.Mentors.API.BogusData;
using Mentors.API.Controllers;
using Mentors.ApplicationCore.DTO;
using Mentors.ApplicationCore.Interfaces.IMongoService;
using Moq;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Controllers
{
    public class MentorshipSubjectsControllerTests
    {
        private readonly Mock<IMentorshipSubjectService> _mockSubjectService;
        private readonly MentorshipSubjectsController _controller;
        private readonly MentorshipSubjectGenerator _subjectData;
        public MentorshipSubjectsControllerTests()
        {
            _mockSubjectService = new Mock<IMentorshipSubjectService>();
            _controller = new MentorshipSubjectsController(_mockSubjectService.Object);
            _subjectData = new MentorshipSubjectGenerator();
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
            var cancellationToken = CancellationToken.None;

            _mockSubjectService
                .Setup(service => service
                .GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(subjects);

            // Act
            var result = await _controller.GetSubjects(cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockSubjectService
                .Setup(service => service
                .GetByIdAsync(subject.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);

            // Act
            var result = await _controller.GetSubject(subject.Id, cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockSubjectService
                .Setup(service => service.CreateAsync(
                    subject, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);

            // Act
            var result = await _controller.CreateSubject(subject, cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockSubjectService
                .Setup(service => service.UpdateAsync(
                    subject, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);

            // Act
            var result = await _controller.UpdateSubject(subject, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteSubject_ShouldReturnNoContent()
        {
            // Arrange
            var subject = _subjectData.GenerateFakeDto();
            var cancellationToken = CancellationToken.None;

            _mockSubjectService
                .Setup(service => service.DeleteAsync(
                    subject.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteSubject(subject.Id, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}