namespace MentorPlatform.Tests.UnitTests.Mentors.API.Helpers.MentorshipSubjects
{
    public class SubjectsControllerHelper
    {
        private readonly Mock<IMentorshipSubjectService> _mockSubjectService;

        public SubjectsControllerHelper(
            Mock<IMentorshipSubjectService> mockSubjectService)
        {
            _mockSubjectService = mockSubjectService;
        }
        public void SetupGetAllAsync(List<MentorshipSubjectDto> subjects)
        {
            _mockSubjectService
                .Setup(service => service.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(subjects);
        }

        public void SetupGetByIdAsync(MentorshipSubjectDto subject)
        {
            _mockSubjectService
                .Setup(service => service.GetByIdAsync(
                    subject.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);
        }

        public void SetupCreateAsync(MentorshipSubjectDto subject)
        {
            _mockSubjectService
                .Setup(service => service.CreateAsync(
                    subject, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);
        }

        public void SetupUpdateAsync(MentorshipSubjectDto subject)
        {
            _mockSubjectService
                .Setup(service => service.UpdateAsync(
                    subject, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);
        }

        public void SetupDeleteAsync(MentorshipSubjectDto subject)
        {
            _mockSubjectService
                .Setup(service => service.DeleteAsync(
                    subject.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }
    }
}