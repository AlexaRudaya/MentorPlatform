namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(
            IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStudents(CancellationToken cancellationToken = default)
        {
            var students = await _studentService.GetAllAsync(cancellationToken);

            return Ok(students);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStudent([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var student = await _studentService.GetByIdAsync(id, cancellationToken);

            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentCreateDto,
            CancellationToken cancellationToken = default)
        {
            var studentToCreate = await _studentService.CreateAsync(studentCreateDto, cancellationToken);

            return CreatedAtAction(
               nameof(GetStudent),
               new { id = studentToCreate.Id },
               studentToCreate);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentDto studentDto,
            CancellationToken cancellationToken = default)
        {
            var studentToUpdate = await _studentService.UpdateAsync(studentDto, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var studentToToDelete = await _studentService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
