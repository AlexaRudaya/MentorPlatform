namespace MentorPlatform.Tests.UnitTests.Chat.API.Helpers
{
    public class ChatHubHelper
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly Mock<IHubCallerClients<IChatClient>> _mockClients;
        private readonly Mock<IChatClient> _mockClientProxy;
        private readonly Mock<HubCallerContext> _mockContext;
        private readonly ChatHub _chatHub;

        public ChatHubHelper(
            Mock<IUserRepository> mockUserRepository,
            Mock<IMessageRepository> mockMessageRepository,
            Mock<IHubCallerClients<IChatClient>> mockClients,
            Mock<IChatClient> mockClientProxy,
            Mock<HubCallerContext> mockContext,
            ChatHub chatHub)
        {
            _mockUserRepository = mockUserRepository;
            _mockMessageRepository = mockMessageRepository;
            _mockClients = mockClients;
            _mockClientProxy = mockClientProxy;
            _mockContext = mockContext;
            _chatHub = chatHub;

        }

        public void SetupGetOneAsync(User user)
        {
            _mockUserRepository
                .Setup(repository => repository.GetOneByAsync(
                    It.IsAny<Func<IQueryable<User>,
                    IIncludableQueryable<User, object>>>(),
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
        }

        public void SetupGetOneAsyncWhenNull()
        {
            _mockUserRepository
                .Setup(repository => repository.GetOneByAsync(
                    It.IsAny<Func<IQueryable<User>,
                    IIncludableQueryable<User, object>>>(),
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);
        }

        public void SetupCreateAsync(Message message)
        {
            _mockMessageRepository
                .Setup(repository => repository.CreateAsync(
                    message, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public void SetupCreateUserAsync(User user) 
        {
            _mockUserRepository
                .Setup(repository => repository.CreateAsync(
                    user, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public void SetupCreateMessageAsync(Message message)
        {
            _mockMessageRepository
                .Setup(repository => repository.CreateAsync(
                    message, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public void SetupAllClients()
        {
            _mockClients
                .Setup(clients => clients.All)
                .Returns(_mockClientProxy.Object);

            _chatHub.Clients = _mockClients.Object;
        }

        public void SetupOthersClients()
        {
            _mockClients
                .Setup(clients => clients.Others)
                .Returns(_mockClientProxy.Object);

            _chatHub.Clients = _mockClients.Object;
        }

        public void SetupConnectionId(string connectionId)
        {
            _mockContext
                .Setup(context => context.ConnectionId)
                .Returns(connectionId);

            _chatHub.Context = _mockContext.Object;
        }
    }
}