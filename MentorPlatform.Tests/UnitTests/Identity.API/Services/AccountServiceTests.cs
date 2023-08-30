using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MentorPlatform.Tests.UnitTests.Identity.API.Services
{
    public class AccountServiceTests
    {
        private readonly UserManager<ApplicationUser> _mockUserManager;
        private readonly SignInManager<ApplicationUser> _mockSignInManager;
        private readonly IMapper _mockMapper;
        private readonly ILogger<AccountService> _mockLogger;
        private readonly IAccountService _accountService;
        private readonly AccountServiceTestsHelper _helper;
        private readonly RegisterDataGenerator _registerData;
        private readonly LoginDataGenerator _loginData;

        public AccountServiceTests()
        {
            _mockUserManager = Substitute.For<UserManager<ApplicationUser>>(
                Substitute.For<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            _mockSignInManager = Substitute.For<SignInManager<ApplicationUser>>(
                _mockUserManager,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            _mockMapper = Substitute.For<IMapper>();
            _mockLogger = Substitute.For<ILogger<AccountService>>();
            _helper = new AccountServiceTestsHelper(_mockUserManager, _mockSignInManager, _mockMapper);
            _registerData = new RegisterDataGenerator();
            _loginData = new LoginDataGenerator();

            _accountService = new AccountService(
                _mockUserManager,
                _mockSignInManager,
                _mockMapper,
                _mockLogger);
        }

        [Fact]
        public async Task RegisterAsync_ValidInput_ShouldReturnSuccess()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            var applicationUser = new ApplicationUser();

            _helper.SetUpValidUserForRegister(applicationUser, registerDto);

            // Act
            await _accountService.RegisterAsync(registerDto, cancellationToken);

            // Assert
            await _mockUserManager
                .Received(1)
                .CreateAsync(applicationUser, registerDto.Password);
        }

        [Fact]
        public async Task RegisterAsync_ShouldLoginUser()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            var applicationUser = new ApplicationUser();

            _helper.SetUpValidUserForRegister(applicationUser, registerDto);

            // Act
            await _accountService.RegisterAsync(registerDto, cancellationToken);

            // Assert
            await _mockSignInManager
                .Received(1)
                .SignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<bool>());
        }

        [Fact]
        public async Task RegisterAsync_NotValidPassword_ShouldLogError()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            var applicationUser = new ApplicationUser();

            _mockMapper
                .Map<ApplicationUser>(registerDto)
                .Returns(applicationUser);

            _mockUserManager
               .CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
               .Returns(IdentityResult.Failed(new IdentityError { Description = "Password is invalid, not unique enough" }));
             
            // Act
            await _accountService.RegisterAsync(registerDto, cancellationToken);

            // Assert
            _mockLogger
                .Received(1)
                .LogError($"Invalid register attempt: User {applicationUser.Email} entered the password that is not unique enough.");

            await _mockSignInManager
                .DidNotReceive()
                .CheckPasswordSignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<bool>());
        }

        [Fact]
        public async Task LoginAsync_ValidAttempt_ShouldReturnSuccess()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            var applicationUser = _helper.SetUpValidUserForLogin(loginDto);

            _mockSignInManager
                .CheckPasswordSignInAsync(applicationUser, loginDto.Password, false)
                .Returns(SignInResult.Success);

            // Act
            await _accountService.LoginAsync(loginDto, cancellationToken);

            // Assert
            _mockLogger
                .Received(1)
                .LogInformation($"User {applicationUser.Email} logged in successfully.");

            await _mockSignInManager
                .Received(1)
                .SignInAsync(applicationUser, true);
        }

        [Fact]
        public async Task LoginAsync_InvalidEmail_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            _mockUserManager
                .FindByEmailAsync(loginDto.Email)
                .Returns((ApplicationUser)null);

            // Act
            var result = async() => await _accountService.LoginAsync(loginDto, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<UserNotFoundException>();

            _mockLogger
                .Received(1)
                .LogError($"User {loginDto.Email} was not found.");
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ShouldThrowInvalidPasswordException()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            var applicationUser = _helper.SetUpValidUserForLogin(loginDto);

            _mockSignInManager
                .CheckPasswordSignInAsync(applicationUser, loginDto.Password, false)
                .Returns(SignInResult.Failed);

            // Act
            var result = async() => await _accountService.LoginAsync(loginDto, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<InvalidPasswordException>();

            _mockLogger
                .Received(1)
                .LogError($"Invalid login attempt: User {applicationUser.Email} password is invalid.");
        }
    }
}