namespace Booking.ApplicationCore.Interfaces.IService
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<BookingDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<BookingDto>> GetBookingsForStudentAsync(Guid id, CancellationToken cancellationToken = default);

        Task<BookingDto> CreateAsync(BookingDto bookingDto, CancellationToken cancellationToken = default);

        Task<BookingDto> UpdateAsync(BookingDto bookingDto, CancellationToken cancellationToken = default);

        Task<BookingDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}