namespace Mentors.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentorshipSubjectsController : ControllerBase
    {
        private readonly IMentorshipSubjectService _subjectService;

        public MentorshipSubjectsController(
            IMentorshipSubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MentorshipSubject>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubjects(CancellationToken cancellationToken = default)
        {
            var subjects = await _subjectService.GetAllAsync(cancellationToken);

            return Ok(subjects);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubject([FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var subject = await _subjectService.GetByIdAsync(id, cancellationToken);

            return Ok(subject);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateSubject([FromBody] MentorshipSubjectDto subjectDto,
            CancellationToken cancellationToken = default)
        {
            var subjectToCreate = await _subjectService.CreateAsync(subjectDto, cancellationToken);

            return CreatedAtAction(
                nameof(GetSubject),
                new { id = subjectDto.Id },
                subjectDto);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSubject([FromBody] MentorshipSubjectDto subjectDto,
            CancellationToken cancellationToken = default)
        {
            await _subjectService.UpdateAsync(subjectDto, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSubject([FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            await _subjectService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}