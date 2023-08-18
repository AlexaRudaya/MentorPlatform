namespace Chat.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}