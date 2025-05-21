using System.Net;
using API.Shared.ErrorModels;
using API.Shared.Exceptions;

namespace API.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                await HandleNotFoundPathAsync(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleNotFoundPathAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound && !httpContext.Response.HasStarted)
            {
                httpContext.Response.ContentType = "application/json";
                var response = new ErrorDetails
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    ErrorMessages = $"The endpoint {httpContext.Request.Path} was not found."
                };

                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }

private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
{
    httpContext.Response.ContentType = "application/json";

    var response = new ErrorDetails
    {
        StatusCode = (int)HttpStatusCode.InternalServerError,
        ErrorMessages = "An unexpected error occurred. Please try again later."
    };

    var statusCode = HttpStatusCode.InternalServerError;

    switch (ex)
    {
        case UserNotFoundException userNotFoundEx:
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.ErrorMessages = userNotFoundEx.Message;
            statusCode = HttpStatusCode.NotFound;
            break;

        case ServiceException serviceEx:
            response.StatusCode = (int)HttpStatusCode.BadRequest; 
            response.ErrorMessages = serviceEx.Message;  
            statusCode = HttpStatusCode.BadRequest;
            break;

        case BadRequestException badRequestEx:
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ErrorMessages = "There were validation errors.";
            response.Errors = badRequestEx.Errors;
            statusCode = HttpStatusCode.BadRequest;
            break;

        case UnauthorizedAccessException unauthorizedEx:
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response.ErrorMessages = unauthorizedEx.Message;
            statusCode = HttpStatusCode.Unauthorized;
            break;

       default:
        var message = ex.InnerException?.Message ?? ex.Message;
        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        response.ErrorMessages = message;
        statusCode = HttpStatusCode.InternalServerError;
        break;
    }

    httpContext.Response.StatusCode = (int)statusCode;

    await httpContext.Response.WriteAsJsonAsync(response);
}


        private int GetValidationErrors(BadRequestException badRequestException, ErrorDetails response)
        {
            response.Errors = badRequestException.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
