using PersonalFinance.Identity.Domain.Exceptions;

namespace PersonalFinance.Identity.Host.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                await HandleDomainExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro não tratado durante o processamento da requisição.");

                await HandleUnexpectedExceptionAsync(context);
            }
        }

        private static async Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = StatusCodes.Status400BadRequest,
                message = exception.Message
            });
        }

        private static async Task HandleUnexpectedExceptionAsync(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = StatusCodes.Status500InternalServerError,
                message = "Ocorreu um erro interno no servidor."
            });
        }
    }
}

