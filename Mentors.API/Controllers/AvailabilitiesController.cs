using Availability = Mentors.Domain.Entities.Availability;

namespace Mentors.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilitiesController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilitiesController(
            IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Availability>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAvailabilities(CancellationToken cancellationToken = default)
        {
            var availabilities = await _availabilityService.GetAllAsync(cancellationToken);

            return Ok(availabilities);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAvailability([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var availability = await _availabilityService.GetByIdAsync(id, cancellationToken);

            return Ok(availability);
        }

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateAvailability([FromBody] AvailabilityDto availabilityDto,
            CancellationToken cancellationToken = default)
        {
            var mentorToCreate = await _availabilityService.CreateAsync(availabilityDto, cancellationToken);

            return Ok("Successfully created");
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateAvailability([FromBody] AvailabilityDto availabilityDto,
            CancellationToken cancellationToken = default)
        {
            var availabilityToUpdate = await _availabilityService.UpdateAsync(availabilityDto, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAvailability([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var availabilityToToDelete = await _availabilityService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}