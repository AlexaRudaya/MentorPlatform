namespace MentorPlatform.Tests.UnitTests.Mentors.API.Helpers.MentorshipSubjects
{
    public class SubjectServiceHelper
    {
        private readonly Mock<IMentorshipSubjectRepository> _mockSubjectRepository;
        private readonly Mock<IMapper> _mockMapper;

        public SubjectServiceHelper(
            Mock<IMentorshipSubjectRepository> mockSubjectRepository,
            Mock<IMapper> mockMapper)
        {
            _mockSubjectRepository = mockSubjectRepository;
            _mockMapper = mockMapper;
        }

        public void SetupGetAllAsync(List<MentorshipSubject> subjects)
        {
            _mockSubjectRepository
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(subjects);
        }

        public void SetupGetAllAsyncWhenNull()
        {
            _mockSubjectRepository
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<MentorshipSubject>)null);
        }

        public void SetupGetByIdAsync(MentorshipSubject subject)
        {
            _mockSubjectRepository
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);
        }

        public void SetupGetByIdAsyncWhenNull()
        {
            _mockSubjectRepository
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((MentorshipSubject)null);
        }

        public void SetupCreateAsync(MentorshipSubject subject)
        {
            _mockSubjectRepository
                .Setup(repository => repository.CreateAsync(
                    subject,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public List<MentorshipSubjectDto> GenerateDtoList(IEnumerable<MentorshipSubject> subjects)
        {
            return subjects.Select(subject => new MentorshipSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            }).ToList();
        }

        public MentorshipSubjectDto GenerateDtoFromSubject(MentorshipSubject subject)
        {
            return new MentorshipSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            };
        }

        public void SetupMapperForSubjectToDto(MentorshipSubject subject,
            MentorshipSubjectDto subjectDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<MentorshipSubjectDto>(subject))
                .Returns(subjectDto);
        }

        public void SetupMapperForDtoToSubject(MentorshipSubject subject,
            MentorshipSubjectDto subjectDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<MentorshipSubject>(subjectDto))
                .Returns(subject);
        }
    }
}