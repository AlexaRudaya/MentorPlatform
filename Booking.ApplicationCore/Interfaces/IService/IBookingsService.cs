namespace Booking.ApplicationCore.Interfaces.IService
{
    public interface IBookingsService
    {
        Task<IEnumerable<BookingsDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<BookingsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<BookingsDto>> GetBookingsForStudentAsync(Guid id, CancellationToken cancellationToken = default);

        Task<BookingsDto> CreateAsync(BookingsDto bookingDto, CancellationToken cancellationToken = default);

        Task<BookingsDto> UpdateAsync(BookingsDto bookingDto, CancellationToken cancellationToken = default);

        Task<BookingsDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}