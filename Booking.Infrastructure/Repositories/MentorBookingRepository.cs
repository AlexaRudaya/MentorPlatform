namespace Booking.Infrastructure.Repositories
{
    public class MentorBookingRepository : BaseRepository<MentorBooking>, IMentorBookingRepository
    {
        public MentorBookingRepository(BookingDbContext dbContext) : base(dbContext)
        {
        }
    }
}