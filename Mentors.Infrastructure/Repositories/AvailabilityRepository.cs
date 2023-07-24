namespace Mentors.Infrastructure.Repositories
{
    public class AvailabilityRepository : BaseRepository<Availability>, IAvailabilityRepository
    {
        public AvailabilityRepository(MentorDbContext dbContext) : base(dbContext)
        {
        }
    }
}