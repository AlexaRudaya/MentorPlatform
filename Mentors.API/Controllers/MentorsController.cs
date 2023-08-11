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

            return CreatedAtAction(
                nameof(GetMentor),
                new { id = mentorToCreate.Id },
                mentorToCreate);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateMentor([FromBody] MentorDto mentorDto,
            CancellationToken cancellationToken = default)
        {
            var mentorToUpdate = await _mentorService.UpdateAsync(mentorDto, cancellationToken);

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