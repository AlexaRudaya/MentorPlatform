namespace MentorPlatform.Tests.UnitTests.Chat.API.Hubs
{
    public class ChatHubTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly Mock<ILogger<ChatHub>> _mockLogger;
        private readonly MessageGenerator _messageGenerator;
        private readonly UserGenerator _userGenerator;
        private readonly Mock<IHubCallerClients<IChatClient>> _mockClients;
        private readonly Mock<IChatClient> _mockClientProxy;
        private readonly Mock<HubCallerContext> _mockContext;
        private readonly ChatHub _chatHub;
        private readonly ChatHubHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public ChatHubTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMessageRepository = new Mock<IMessageRepository>();
            _mockLogger = new Mock<ILogger<ChatHub>>();
            _messageGenerator = new MessageGenerator();
            _userGenerator = new UserGenerator();
            _mockClients = new Mock<IHubCallerClients<IChatClient>>();
            _mockClientProxy = new Mock<IChatClient>();
            _mockContext = new Mock<HubCallerContext>();
            _chatHub = new ChatHub(
                _mockUserRepository.Object,
                _mockMessageRepository.Object,
                _mockLogger.Object);
            _helper = new ChatHubHelper(
                _mockUserRepository,
                _mockMessageRepository,
                _mockClients,
                _mockClientProxy,
                _mockContext,
                _chatHub);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task SendMessage_WhenCalled_ShouldCreateAndSendMessage()
        {
            // Arrange
            var user = _userGenerator.GenerateFakeUser();
            var message = _messageGenerator.GenerateFakeMessage();
            message.UserId = user.Id;

            _helper.SetupGetOneAsync(user);
            _helper.SetupCreateAsync(message);
            _helper.SetupAllClients();

            // Act
            await _chatHub.SendMessage(user.Name, message.Content);

            // Assert
            _mockMessageRepository
                .Verify(repository => repository
                    .CreateAsync(
                        It.IsAny<Message>(),
                        _cancellationToken),
                Times.Once);

            _mockClientProxy
                .Verify(client => client.ReceiveMessage(
                    user.Name,
                    message.Content));
        }

        [Fact]
        public async Task JoinChat_WhenUserNotExists_ShouldCreateUserAndSendMessage()
        {
            // Arrange
            var user = _userGenerator.GenerateFakeUser();
            var message = _messageGenerator.GenerateFakeMessage();
            message.UserId = user.Id;
            var connectionId = "testId";

            _helper.SetupGetOneAsyncWhenNull();
            _helper.SetupCreateUserAsync(user);
            _helper.SetupCreateMessageAsync(message);

            _helper.SetupConnectionId(connectionId);
            _helper.SetupOthersClients();

            // Act
            await _chatHub.JoinChat(user.Name, message.Content);

            // Assert
            _mockUserRepository
                .Verify(repository => repository.CreateAsync(
                    It.Is<User>(u => u.Name == user.Name),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _mockMessageRepository
                .Verify(repository => repository
                    .CreateAsync(
                        It.IsAny<Message>(),
                        _cancellationToken),
                Times.Once);

            _mockClientProxy
                .Verify(client => client.ReceiveMessage(
                    user.Name,
                    message.Content));
        }

        [Fact]
        public async Task LeaveChat_WhenUserIsFound_ShouldSendLeavingMessage()
        {
            // Arrange
            var user = _userGenerator.GenerateFakeUser();
            var message = _messageGenerator.GenerateFakeMessage();
            var expectedLeavingMessage = $"{user.Name} left the chat";
            var connectionId = "testId";

            _helper.SetupConnectionId(connectionId);
            _helper.SetupOthersClients();

            await _chatHub.JoinChat(user.Name, message.Content);

            // Act
            await _chatHub.LeaveChat();

            // Assert
            _mockClientProxy
                .Verify(client => client.ReceiveMessage(
                    user.Name, expectedLeavingMessage),
                Times.Once);
        }

        [Fact]
        public async Task OnDisconnectedAsync_WhenCalled_ShouldExecuteLeaveChatLogicAndCloseConnection()
        {
            // Arrange
            var user = _userGenerator.GenerateFakeUser();
            var message = _messageGenerator.GenerateFakeMessage();
            var expectedLeavingMessage = $"{user.Name} left the chat";
            var connectionId = "testId";

            _helper.SetupConnectionId(connectionId);
            _helper.SetupOthersClients();

            await _chatHub.JoinChat(user.Name, message.Content);

            // Act
            await _chatHub.OnDisconnectedAsync(null);

            // Assert
            _mockClientProxy
                .Verify(client => client.ReceiveMessage(
                    user.Name, expectedLeavingMessage),
                Times.Once);
        }
    }
}