namespace Mentors.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentorsController : ControllerBase
    {
        private readonly IMentorService _mentorService;

        public MentorsController(
            IMentorService mentorService)
        {
            _mentorService = mentorService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Mentor>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMentors(CancellationToken cancellationToken = default)
        {
            var mentors = await _mentorService.GetAllAsync(cancellationToken);

            return Ok(mentors);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMentor([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var mentor = await _mentorService.GetByIdAsync(id, cancellationToken);

            return Ok(mentor);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateMentor([FromBody] MentorCreateDto mentorDto,
            CancellationToken cancellationToken = default)
        {
            var mentorToCreate = await _mentorService.CreateAsync(mentorDto, cancellationToken);

            return Ok("Successfully created");
        }

        [Authorize]
        [HttpPatch("{id:Guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateMentor([FromRoute] Guid id,
            [FromBody] MentorDto mentorDto,
            CancellationToken cancellationToken = default)
        {
            var mentorToUpdate = await _mentorService.UpdateAsync(id, mentorDto, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMentor([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var mentorToDelete = await _mentorService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}