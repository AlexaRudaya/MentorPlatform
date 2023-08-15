using Chat.Domain.Entities;
using Chat.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;

        public ChatHub(
            ChatDbContext context)
        {
            _context = context; 
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

        public async Task JoinChat(string content)
        {
            var userId = Guid.Parse(Context.UserIdentifier);
            var user = await _context.Users.FindAsync(userId);

            await Groups.AddToGroupAsync(Context.ConnectionId, "ChatRoom");

            var message = new Message
            {
                Content = content,
                UserId = userId,
                User = user
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await Clients.Others.SendAsync("ReceiveMessage", user, content);
        }
    }
}