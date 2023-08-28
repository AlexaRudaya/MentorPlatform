using Identity.ApplicationCore.DTO;
using Identity.ApplicationCore.Services;
using MentorPlatform.Tests.UnitTests.Identity.API.BogusData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MentorPlatform.Tests.UnitTests.Identity.API.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<AccountService>> _mockLogger;
        private readonly IAccountService _accountService;
        private readonly RegisterDataGenerator _registerData;  

        public AccountServiceTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), 
                null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AccountService>>();
            _registerData = new RegisterDataGenerator();

            _accountService = new AccountService(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task RegisterAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            var applicationUser = new ApplicationUser();

            _mockMapper
                .Setup(mapper => mapper.Map<ApplicationUser>(registerDto))
                .Returns(applicationUser);

            _mockUserManager
                .Setup(manager => manager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _accountService.RegisterAsync(registerDto, cancellationToken);

            // Assert
            _mockUserManager.Verify(
                userManager => userManager.CreateAsync(applicationUser, registerDto.Password),
                Times.Once);

            _mockSignInManager.Verify(
                signInManager => signInManager.SignInAsync(applicationUser, true, null),
                Times.Once);
        }
    }
}