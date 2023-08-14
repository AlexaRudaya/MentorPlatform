using Mentors.ApplicationCore.Interfaces.IMongoService;
using Mentors.Domain.Abstractions.IRepository.IMongoRepository;

namespace Mentors.ApplicationCore.Services.MongoServices
{
    public class MentorshipSubjectService : IMentorshipSubjectService
    {
        private readonly IMentorshipSubjectRepository _subjectRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MentorshipSubjectService> _logger;

        public MentorshipSubjectService(
            IMentorshipSubjectRepository subjectRepository,
            IMapper mapper,
            ILogger<MentorshipSubjectService> logger)
        {
            _subjectRepository = subjectRepository; 
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MentorshipSubjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var allSubjects = await _subjectRepository.GetAllAsync(cancellationToken);

            var subjectsDto = _mapper.Map<IEnumerable<MentorshipSubjectDto>>(allSubjects);

            if (allSubjects is null)
            {
                throw new MentorshipSubjectNotFoundException();
            }

            _logger.LogInformation("Mentorship subjects are loaded");

            return subjectsDto;
        }

        public async Task<MentorshipSubjectDto> GetByIdAsync(Guid subjectId, CancellationToken cancellationToken = default)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId, cancellationToken);

            var subjectDto = _mapper.Map<MentorshipSubjectDto>(subject);

            if (subject is null)
            {
                throw new MentorshipSubjectNotFoundException(subjectId);
            }

            return subjectDto;
        }

        public async Task<MentorshipSubjectDto> CreateAsync(MentorshipSubjectDto subjectDto, 
            CancellationToken cancellationToken = default)
        {
            var subjectToCreate = _mapper.Map<MentorshipSubject>(subjectDto);

            await _subjectRepository.CreateAsync(subjectToCreate, cancellationToken);

            _logger.LogInformation($"A mentorship subject with Id:{subjectToCreate.Id} and Name:{subjectToCreate.Name} is created successfully");

            return subjectDto;
        }

        public async Task<MentorshipSubjectDto> UpdateAsync(MentorshipSubjectDto subjectDto,
            CancellationToken cancellationToken = default)
        {
            var existingSubject = await _subjectRepository.GetByIdAsync(subjectDto.Id);

            if (existingSubject is null)
            {
                _logger.LogError($"Failed finding subject with Id:{subjectDto.Id} while updating entity.");
                throw new MentorshipSubjectNotFoundException(subjectDto.Id);
            }

            var subjectToUpdate = _mapper.Map<MentorshipSubject>(subjectDto);

            await _subjectRepository.UpdateAsync(subjectToUpdate, cancellationToken);

            _logger.LogInformation($"Data for subject with Id: {existingSubject.Id} has been successfully updated.");

            return subjectDto;
        }

        public async Task DeleteAsync(Guid subjectId, CancellationToken cancellationToken = default)
        {
            await _subjectRepository.DeleteAsync(subjectId, cancellationToken);

            _logger.LogInformation($"Subject with Id: {subjectId} is removed");
        }
    }
}