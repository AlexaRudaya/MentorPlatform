namespace Booking.Infrastructure.Repositories
{
    public class BookingsRepository : BaseRepository<Bookings>, IBookingsRepository
    {
        public BookingsRepository(BookingDbContext dbContext) : base(dbContext)
        {
        }
    }
}