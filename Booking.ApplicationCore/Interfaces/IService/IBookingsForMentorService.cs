namespace Booking.ApplicationCore.Interfaces.IService
{
    public interface IBookingsForMentorService
    {
        Task<IEnumerable<BookingsDto>> GetBookingsForMentorAsync(string mentorId, CancellationToken cancellationToken = default);

        Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesOfMentor(string mentorId, CancellationToken cancellationToken = default);
    }
}