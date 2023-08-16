﻿namespace Mentors.ApplicationCore.Services.MongoServices
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

            if (allSubjects is null)
            {
                throw new MentorshipSubjectNotFoundException();
            }

            var subjectsDto = _mapper.Map<IEnumerable<MentorshipSubjectDto>>(allSubjects);

            _logger.LogInformation("Mentorship subjects are loaded");

            return subjectsDto;
        }

        public async Task<MentorshipSubjectDto> GetByIdAsync(string subjectId, CancellationToken cancellationToken = default)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId, cancellationToken);

            if (subject is null)
            {
                throw new MentorshipSubjectNotFoundException(subjectId);
            }

            var subjectDto = _mapper.Map<MentorshipSubjectDto>(subject);

            return subjectDto;
        }

        public async Task<MentorshipSubjectDto> CreateAsync(MentorshipSubjectDto subjectDto, 
            CancellationToken cancellationToken = default)
        {
            var subjectToCreate = _mapper.Map<MentorshipSubject>(subjectDto);

            await _subjectRepository.CreateAsync(subjectToCreate, cancellationToken);

            _logger.LogInformation($"A mentorship subject with Id:{subjectToCreate.Id} is created successfully");

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

        public async Task DeleteAsync(string subjectId, CancellationToken cancellationToken = default)
        {
            await _subjectRepository.DeleteAsync(subjectId, cancellationToken);

            _logger.LogInformation($"Subject with Id: {subjectId} is removed");
        }
    }
}