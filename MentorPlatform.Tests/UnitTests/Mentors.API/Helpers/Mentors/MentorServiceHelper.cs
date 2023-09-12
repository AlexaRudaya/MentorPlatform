using MentorDto = Mentors.ApplicationCore.DTO.MentorDto;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Helpers.Mentors
{
    public class MentorServiceHelper
    {
        private readonly Mock<IMentorRepository> _mockMentorRepository;
        private readonly Mock<IMapper> _mockMapper;

        public MentorServiceHelper(
            Mock<IMentorRepository> mockMentorRepository,
            Mock<IMapper> mockMapper)
        {
            _mockMentorRepository = mockMentorRepository;
            _mockMapper = mockMapper;
        }

        public void SetupGetAllAsync(List<Mentor> mentors)
        {
            _mockMentorRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentors);
        }

        public void SetupGetAllAsyncWhenNull()
        {
            _mockMentorRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Mentor>)null);
        }

        public void SetupGetByIdAsync(Mentor mentor)
        {
            _mockMentorRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Mentor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);
        }

        public void SetupGetByIdAsyncWhenNull()
        {
            _mockMentorRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Mentor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Mentor)null);
        }

        public void SetupCreateAsync(Mentor mentor)
        {
            _mockMentorRepository
                .Setup(repository => repository.CreateAsync(
                    mentor,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public List<MentorDto> GenerateDtoList(IEnumerable<Mentor> mentors)
        {
            return mentors.Select(mentor => new MentorDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            }).ToList();
        }

        public MentorDto GenerateDtoFromMentor(Mentor mentor)
        {
            return new MentorDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };
        }

        public MentorCreateDto GenerateCreateDtoFromMentor(Mentor mentor)
        {
            return new MentorCreateDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };

        }

        public void SetupMapperForMentorToDto(Mentor mentor,
            MentorDto mentorDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<MentorDto>(mentor))
                .Returns(mentorDto);
        }

        public void SetupMapperForDtoToMentor(Mentor mentor,
            MentorDto mentorDto)
        {
            _mockMapper
                 .Setup(mapper => mapper.Map<Mentor>(mentorDto))
                 .Returns(mentor);
        }

        public void SetupMapperForMentorToCreateDto(Mentor mentor,
           MentorCreateDto mentorDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<MentorCreateDto>(mentor))
                .Returns(mentorDto);
        }

        public void SetupMapperForCreateDtoToMentor(Mentor mentor,
            MentorCreateDto mentorDto)
        {
            _mockMapper
                 .Setup(mapper => mapper.Map<Mentor>(mentorDto))
                 .Returns(mentor);
        }
    }
}