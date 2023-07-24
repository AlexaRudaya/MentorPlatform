namespace Mentors.API.Middlewares
{
    public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) when (ex is CategoryNotFoundException || ex is AvailabilityNotFoundException
                                       || ex is MentorNotFoundException)
            {
                _logger.LogError($"Not Found exception has occured: {ex}");
                await HandleExceptionAsync(context, ex);
            }
            catch (InvalidValueException ex)
            {
                _logger.LogError($"Invalid value exception has occured: {ex}");
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;

            statusCode = exception switch
            {
                CategoryNotFoundException => HttpStatusCode.NotFound,
                AvailabilityNotFoundException => HttpStatusCode.NotFound,
                MentorNotFoundException => HttpStatusCode.NotFound,
                InvalidValueException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new ProblemDetails()
            {
                Status = (int)statusCode,
                Title = exception.GetType().Name,
                Detail = exception.Message,
            };

            await context.Response.WriteAsync(problemDetails.ToString()!);
        }
    }
}