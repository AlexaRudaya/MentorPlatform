namespace Mentors.ApplicationCore.Services
{
    public sealed class MentorService : IMentorService
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MentorService> _logger;

        public MentorService(
            IMentorRepository mentorRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<MentorService> logger)
        {
            _mentorRepository = mentorRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MentorDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var allMentors = await _mentorRepository.GetAllByAsync(cancellationToken: cancellationToken);

            var mentorsDto = _mapper.Map<IEnumerable<MentorDto>>(allMentors);

            if (allMentors is null)
            {
                throw new MentorNotFoundException();
            }

            _logger.LogInformation("Mentors are loaded");

            return mentorsDto;
        }

        public async Task<MentorDto> GetByIdAsync(Guid mentorId, CancellationToken cancellationToken = default)
        {
            var mentor = await _mentorRepository.GetOneByAsync(expression: mentor => mentor.Id.Equals(mentorId),
                                                               cancellationToken: cancellationToken);   

            var mentorDto = _mapper.Map<MentorDto>(mentor);

            if (mentor is null)
            {
                throw new MentorNotFoundException(mentorId);
            }

            return mentorDto;
        }

        public async Task<MentorCreateDto> CreateAsync(MentorCreateDto mentorCreateDto,
            CancellationToken cancellationToken = default)
        {
            var mentorToCreate = _mapper.Map<Mentor>(mentorCreateDto);

            var category = await _categoryRepository.GetOneByAsync(expression: category => category.Id.Equals(mentorCreateDto.CategoryId));

            if (category is not null)
            {
                mentorToCreate.CategoryId = category.Id;
            }
            else
            {
                _logger.LogError($"Failed finding category with Id:{mentorCreateDto.CategoryId}.");
                throw new MentorNotFoundException(mentorCreateDto.CategoryId);
            }

            _mapper.Map(mentorCreateDto.Availabilities, mentorToCreate.Availabilities);

            await _mentorRepository.CreateAsync(mentorToCreate, cancellationToken);

            _logger.LogInformation($"Mentor with Id: {mentorToCreate.Id} is created.");

            var createdMentorDto = _mapper.Map<MentorCreateDto>(mentorToCreate);

            return createdMentorDto;
        }

        public async Task<MentorDto> UpdateAsync(MentorDto mentorDto,
            CancellationToken cancellationToken = default)
        {
            var existingMentor = await _mentorRepository.GetOneByAsync(expression: mentor => mentor.Id.Equals(mentorDto.Id));

            if (existingMentor is null)
            {
                _logger.LogError($"Failed finding mentor with Id:{mentorDto.Id} while updating entity.");
                throw new MentorNotFoundException(mentorDto.Id);
            }

            var mentorToUpdate = _mapper.Map<Mentor>(mentorDto);

            await _mentorRepository.UpdateAsync(mentorToUpdate, cancellationToken);

            _logger.LogInformation($"Data for Mentor with Id: {existingMentor.Id} has been successfully updated.");

            return mentorDto;
        }

        public async Task<MentorDto> DeleteAsync(Guid mentorId, CancellationToken cancellationToken = default)
        {
            var mentorToDelete = await _mentorRepository.GetOneByAsync(expression: mentor => mentor.Id.Equals(mentorId));

            if (mentorToDelete is null)
            {
                throw new MentorNotFoundException(mentorId);
            }

            await _mentorRepository.DeleteAsync(mentorToDelete, cancellationToken);

            _logger.LogInformation($"Mentor with Id: {mentorId} is removed");

            var mentorDeleted = _mapper.Map<MentorDto>(mentorToDelete);

            return mentorDeleted;
        }
    }
}