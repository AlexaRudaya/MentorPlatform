using FluentAssertions;
using Identity.API.Controllers;
using MentorPlatform.Tests.UnitTests.Identity.API.BogusData;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace MentorPlatform.Tests.UnitTests.Identity.API.Controllers
{
    public class AccountsControllerTests
    {
        private readonly AccountsController _controller;
        private readonly IAccountService _mockAccountService;
        private readonly RegisterDataGenerator _registerData;
        private readonly LoginDataGenerator _loginData;

        public AccountsControllerTests()
        {
            _mockAccountService = Substitute.For<IAccountService>();
            _controller = new AccountsController(_mockAccountService);
            _registerData = new RegisterDataGenerator();
            _loginData = new LoginDataGenerator();
        }

        [Fact]
        public async Task Register_ShouldReturnOk()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            await _mockAccountService.RegisterAsync(registerDto, cancellationToken);

            // Act
            var result = await _controller.Register(registerDto, cancellationToken);

            // Assert
            result
                .Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Login_ShouldReturnNoContent()
        { 
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            await _mockAccountService.LoginAsync(loginDto, cancellationToken);

            // Act
            var result = await _controller.Login(loginDto, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>(); 
        }
    }
}