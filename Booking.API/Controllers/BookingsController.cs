namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;
        private readonly IBookingsForMentorService _bookingsForMentorService;

        public BookingsController(
             IBookingsService bookingsService,
             IBookingsForMentorService bookingsForMentorService)
        {
            _bookingsService = bookingsService;
            _bookingsForMentorService = bookingsForMentorService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Bookings>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBookings(CancellationToken cancellationToken = default)
        {
            var bookings = await _bookingsService.GetAllAsync(cancellationToken);

            return Ok(bookings);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBooking([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var booking = await _bookingsService.GetByIdAsync(id, cancellationToken);

            return Ok(booking);
        }

        [HttpGet("student/{id:Guid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Bookings>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBookingsForStudent([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var bookingsForStudent = await _bookingsService.GetBookingsForStudentAsync(id, cancellationToken);

            return Ok(bookingsForStudent);
        }

        [HttpGet("mentor/{id:Guid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Bookings>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBookingsForMentor([FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var bookingsForMentor = await _bookingsForMentorService.GetBookingsForMentorAsync(id, cancellationToken);

            return Ok(bookingsForMentor);
        }

        [HttpGet("mentor/availabilities/{id:Guid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Availability>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAvailabilitiesOfMentor([FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var availabilitiesOfMentor = await _bookingsForMentorService.GetAvailabilitiesOfMentor(id, cancellationToken);

            return Ok(availabilitiesOfMentor);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingsDto bookingDto,
            CancellationToken cancellationToken = default)
        {
            var bookingToCreate = await _bookingsService.CreateAsync(bookingDto, cancellationToken);

            return CreatedAtAction(
                nameof(GetBooking),
                new { id = bookingToCreate.Id },
                bookingToCreate);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory([FromBody] BookingsDto bookingDto,
            CancellationToken cancellationToken = default)
        {
            var bookingToUpdate = await _bookingsService.UpdateAsync(bookingDto, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var bookingToDelete = await _bookingsService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}