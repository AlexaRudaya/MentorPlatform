using Chat.Domain.Entities;
using Chat.Domain.IRepository;
using Chat.Infrastructure.Data;

namespace Chat.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}