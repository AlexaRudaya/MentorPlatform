using Mentors.ApplicationCore.Interfaces.IProducer;
using Mentors.ApplicationCore.MassTransitEvents;
using Availability = Mentors.Domain.Entities.Availability;

namespace Mentors.ApplicationCore.Services
{
    public sealed class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AvailabilityService> _logger;
        private readonly IProducer _producer;

        public AvailabilityService(
            IAvailabilityRepository availabilityRepository,
            IMapper mapper,
            ILogger<AvailabilityService> logger,
            IProducer producer)
        {
            _availabilityRepository = availabilityRepository;
            _mapper = mapper;
            _logger = logger;
            _producer = producer;
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var allAvailabilities = await _availabilityRepository.GetAllByAsync(cancellationToken: cancellationToken);

            if (allAvailabilities is null)
            {
                throw new AvailabilityNotFoundException();
            }

            var availabilitiesDto = _mapper.Map<IEnumerable<AvailabilityDto>>(allAvailabilities);

            _logger.LogInformation("Scheduling is loaded");

            return availabilitiesDto;
        }

        public async Task<AvailabilityDto> GetByIdAsync(Guid availabilityId, CancellationToken cancellationToken = default)
        {
            var availability = await _availabilityRepository.GetOneByAsync(expression: availability => availability.Id.Equals(availabilityId),
                                                                           cancellationToken: cancellationToken);

            if (availability is null)
            {
                throw new AvailabilityNotFoundException(availabilityId);
            }

            var availabilityDto = _mapper.Map<AvailabilityDto>(availability);

            return availabilityDto;
        }

        public async Task<AvailabilityDto> CreateAsync(AvailabilityDto availabilityDto,
            CancellationToken cancellationToken = default)
        {
            var availabilityToCreate = _mapper.Map<Availability>(availabilityDto);

            await _availabilityRepository.CreateAsync(availabilityToCreate, cancellationToken);

            _logger.LogInformation($"Availability with Id: {availabilityToCreate.Id} is created.");

            var createdAvailabilityDto = _mapper.Map<AvailabilityDto>(availabilityToCreate);

            await _producer.PublishAsync(
                new AvailabilityOfMentorCreatedEvent
                { 
                    Id = createdAvailabilityDto.Id,
                    Date = createdAvailabilityDto.Date,
                    StartTime = createdAvailabilityDto.StartTime,
                    EndTime = createdAvailabilityDto.EndTime,
                    MentorId = createdAvailabilityDto.MentorId,
                }, cancellationToken);

            return createdAvailabilityDto;
        }

        public async Task<AvailabilityDto> UpdateAsync(AvailabilityDto availabilityDto,
            CancellationToken cancellationToken = default)
        {
            var existingAvailability = await _availabilityRepository.GetOneByAsync(expression: availability => availability.Id.Equals(availabilityDto.Id));

            if (existingAvailability is null)
            {
                _logger.LogError($"Failed finding availability with Id:{availabilityDto.Id} while updating entity.");
                throw new AvailabilityNotFoundException(availabilityDto.Id);
            }

            var availabilityToUpdate = _mapper.Map<Availability>(availabilityDto);

            await _availabilityRepository.UpdateAsync(availabilityToUpdate, cancellationToken);

            _logger.LogInformation($"Data for availability with Id: {existingAvailability.Id} has been successfully updated.");

            return availabilityDto;
        }

        public async Task<AvailabilityDto> DeleteAsync(Guid availabilityId, CancellationToken cancellationToken = default)
        {
            var availabilityToDelete = await _availabilityRepository.GetOneByAsync(expression: availability => availability.Id.Equals(availabilityId));

            if (availabilityToDelete is null)
            {
                _logger.LogError($"Failed finding availability with Id:{availabilityId} while deleting entity.");
                throw new AvailabilityNotFoundException(availabilityId);
            }

            var availabilityDeleted = _mapper.Map<AvailabilityDto>(availabilityToDelete);

            await _availabilityRepository.DeleteAsync(availabilityToDelete, cancellationToken);

            _logger.LogInformation($"Availability with Id: {availabilityId} is removed");

            return availabilityDeleted;
        }
    }
}