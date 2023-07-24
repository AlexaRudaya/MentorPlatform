namespace Mentors.Infrastructure.Repositories
{
    public class MentorRepository : BaseRepository<Mentor>, IMentorRepository
    {
        public MentorRepository(MentorDbContext dbContext) : base(dbContext)
        {
        }
    }
}