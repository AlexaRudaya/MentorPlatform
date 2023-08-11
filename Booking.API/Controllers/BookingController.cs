namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IBookingForMentorService _bookingForMentorService;

        public BookingController(
             IBookingService bookingService,
             IBookingForMentorService bookingForMentorService)
        {
            _bookingService = bookingService;
            _bookingForMentorService = bookingForMentorService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MentorBooking>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBookings(CancellationToken cancellationToken = default)
        {
            var bookings = await _bookingService.GetAllAsync(cancellationToken);

            return Ok(bookings);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBooking([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var booking = await _bookingService.GetByIdAsync(id, cancellationToken);

            return Ok(booking);
        }

        [HttpGet("student/{id:Guid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MentorBooking>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBookingsForStudent([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var bookingsForStudent = await _bookingService.GetBookingsForStudentAsync(id, cancellationToken);

            return Ok(bookingsForStudent);
        }

        [HttpGet("mentor/{id:Guid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MentorBooking>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBookingsForMentor([FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var bookingsForMentor = await _bookingForMentorService.GetBookingsForMentorAsync(id, cancellationToken);

            return Ok(bookingsForMentor);
        }

        [HttpGet("mentor/availabilities/{id:Guid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Availability>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAvailabilitiesOfMentor([FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var availabilitiesOfMentor = _bookingForMentorService.GetAvailabilitiesOfMentor(id, cancellationToken);

            return Ok(availabilitiesOfMentor);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingDto,
            CancellationToken cancellationToken = default)
        {
            var bookingToCreate = await _bookingService.CreateAsync(bookingDto, cancellationToken);

            return CreatedAtAction(
                nameof(GetBooking),
                new { id = bookingToCreate.Id },
                bookingToCreate);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBooking([FromBody] BookingDto bookingDto,
            CancellationToken cancellationToken = default)
        {
            var bookingToUpdate = await _bookingService.UpdateAsync(bookingDto, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBooking([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var bookingToDelete = await _bookingService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}