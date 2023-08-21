using AutoMapper;
using Chat.ApplicationCore.DTO;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatHub> _logger;
        private static readonly Dictionary<string, string> _connectedUsers = new();

        public ChatHub(
            IUserRepository userRepository,
            IMessageRepository messageRepository,
            IMapper mapper,
            ILogger<ChatHub> logger)
        {
            _userRepository = userRepository; 
            _messageRepository = messageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SendMessage(MessageDto messageDto)
        {
            var user = await _userRepository.GetOneByAsync(expression: user => user.Name == messageDto.User.Name);

            var message = _mapper.Map<Message>(messageDto);

            await _messageRepository.CreateAsync(message);

            await Clients.All.SendAsync("ReceiveMessage", messageDto.User, messageDto.Content);
        }

        public async Task JoinChat(MessageDto messageDto)
        {
            var user = await _userRepository.GetOneByAsync(expression: user => user.Name == messageDto.User.Name);

            if (user is null)
            {
                user = _mapper.Map<User>(messageDto.User);
                await _userRepository.CreateAsync(user);

                _logger.LogInformation($"User with Id: {user.Id} was created");
            }

            var message = _mapper.Map<Message>(messageDto);

            await _messageRepository.CreateAsync(message);

            _connectedUsers[Context.ConnectionId] = messageDto.User.Name;

            await Clients.Others.SendAsync("ReceiveMessage", user, messageDto.Content);
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