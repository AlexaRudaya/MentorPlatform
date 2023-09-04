using Mentors.API.Middlewares;
using Mentors.ApplicationCore.Exceptions;
using Moq;
using System.Net;
using InvalidValueException = Mentors.ApplicationCore.Exceptions.InvalidValueException;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Middlewares
{
    public class GlobalExceptionHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<GlobalExceptionHandlingMiddleware>> _mockLogger;
        private readonly GlobalExceptionHandlingMiddleware _middleware;

        public GlobalExceptionHandlingMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
            _middleware = new GlobalExceptionHandlingMiddleware(_mockLogger.Object);
        }

        [Fact]
        public async Task InvokeAsync_ObjectNotFoundException_ShouldReturnNotFound()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var response = context.Response;

            RequestDelegate next = (innerContext) => throw new ObjectNotFoundException("No entity was found");

            // Act
            await _middleware.InvokeAsync(context, next);

            // Assert
            response.StatusCode
                .Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task InvokeAsync_InvalidValueException_ShouldReturnBadRequest()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var response = context.Response;

            RequestDelegate next = (innerContext) => throw new InvalidValueException("Invalid data");

            // Act
            await _middleware.InvokeAsync(context, next);

            // Assert
            response.StatusCode
                .Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task InvokeAsync_GeneralException_ShouldReturnInternalServerError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var response = context.Response;

            RequestDelegate next = (innerContext) => throw new Exception();

            // Act
            await _middleware.InvokeAsync(context, next);

            // Assert
            response.StatusCode
                .Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}