using Chat.Domain.Entities;
using Chat.Domain.IRepository;
using Chat.Infrastructure.Data;

namespace Chat.Infrastructure.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}