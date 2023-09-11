using Chat.API.Interfaces;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<ChatHub> _logger;
        private static readonly Dictionary<string, string> _connectedUsers = new();

        public ChatHub(
            IUserRepository userRepository,
            IMessageRepository messageRepository,
            ILogger<ChatHub> logger)
        {
            _userRepository = userRepository; 
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task SendMessage(string userName, string content)
        {
            var user = await _userRepository.GetOneByAsync(expression: user => user.Name == userName);

            var message = new Message
            {
                Content = content,
                UserId = user.Id
            };

            await _messageRepository.CreateAsync(message);

            await Clients.All.ReceiveMessage(userName, content);
        }

        public async Task JoinChat(string userName, string content)
        {
            var user = await _userRepository.GetOneByAsync(expression: user => user.Name == userName);

            if (user is null)
            {
                user = new User { Name = userName };
                await _userRepository.CreateAsync(user);

                _logger.LogInformation($"User with Id: {user.Id} was created");
            }

            var message = new Message
            {
                Content = content,
                UserId = user.Id
            };

            await _messageRepository.CreateAsync(message);

            _connectedUsers[Context.ConnectionId] = userName;

            await Clients.Others.ReceiveMessage(userName, content);
        }

        public async Task LeaveChat()
        {
            if (_connectedUsers.TryGetValue(Context.ConnectionId, out string userName))
            {
                var message = $"{userName} left the chat";
                await Clients.Others.ReceiveMessage(userName, message);

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