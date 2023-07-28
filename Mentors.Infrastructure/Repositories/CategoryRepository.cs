namespace Mentors.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MentorDbContext dbContext) : base(dbContext)
        {
        }
    }
}