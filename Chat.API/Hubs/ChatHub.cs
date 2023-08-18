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
        private static readonly Dictionary<string, string> _connectedUsers = new();

        public ChatHub(
            ChatDbContext context,
            ILogger<ChatHub> logger)
        {
            _context = context; 
            _logger = logger;
        }

        public async Task SendMessage(string userName, string content)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Name == userName);

            var message = new Message
            {
                Content = content,
                UserId = user.Id
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", userName, content);
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

            _connectedUsers[Context.ConnectionId] = userName;

            await Clients.Others.SendAsync("ReceiveMessage", userName, content);
        }

        public async Task LeaveChat()
        {
            if (_connectedUsers.TryGetValue(Context.ConnectionId, out string userName))
            {
                var message = $"{userName} left the chat";
                await Clients.Others.SendAsync("ReceiveMessage", userName, message);

                _logger.LogInformation($"User {userName} is leaving the chat.");

                _connectedUsers.Remove(Context.ConnectionId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await LeaveChat();
            await base.OnDisconnectedAsync(exception);

        }
    }
}