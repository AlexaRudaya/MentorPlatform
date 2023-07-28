namespace Booking.Infrastructure.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(BookingDbContext dbContext) : base(dbContext)
        {
        }
    }
}