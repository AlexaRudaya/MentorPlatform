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


        [Theory]
        [InlineData(typeof(ObjectNotFoundException), (int)HttpStatusCode.NotFound)]
        [InlineData(typeof(InvalidValueException), (int)HttpStatusCode.BadRequest)]
        [InlineData(typeof(Exception), (int)HttpStatusCode.InternalServerError)]
        public async Task InvokeAsync_ExceptionType_ShouldReturnExpectedStatusCode(Type exceptionType, int statusCode)
        {
            // Arrange
            var context = new DefaultHttpContext();
            var response = context.Response;

            RequestDelegate next = (innerContext) =>
            {
                if (exceptionType == typeof(ObjectNotFoundException))
                    throw new ObjectNotFoundException("No entity was found");
                if (exceptionType == typeof(InvalidValueException))
                    throw new InvalidValueException("Invalid data");
                throw new Exception();
            };

            // Act
            await _middleware.InvokeAsync(context, next);

            // Assert
            response.StatusCode
                .Should().Be(statusCode);
        }
    }
}