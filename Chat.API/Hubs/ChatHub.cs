using Chat.Domain.Entities;
using Chat.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(
            ChatDbContext context,
            ILogger<ChatHub> logger)
        {
            _context = context; 
            _logger = logger;
        }

        public async Task SendMessage(string content)
        {
            var userId = Guid.Parse(Context.UserIdentifier);
            var user  = await _context.Users.FindAsync(userId);

            var message = new Message
            {
                Content = content,
                UserId = userId,
                User = user 
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", user, content);
        }

        public async Task JoinChat(string userName, string content)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Name == userName);

            if (user is null)
            {
                _logger.LogInformation("User is not found");

                user = new User { Name = userName };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var message = new Message
            {
                Content = content,
                UserId = user.Id 
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await Clients.Others.SendAsync("ReceiveMessage", userName, content);
        }
    }
}