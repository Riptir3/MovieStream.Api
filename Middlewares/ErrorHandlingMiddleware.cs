using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using TaskManager.Api.Models;

namespace TaskManager.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleInternalServerError(context, ex);
            }

            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status403Forbidden:
                    await HandleForbidden(context);
                    break;

                case StatusCodes.Status401Unauthorized:
                    await HandleUnauthorized(context);
                    break;
                case StatusCodes.Status404NotFound:
                    await HandleNotFound(context);
                    break;
            }
        }

        private static async Task HandleInternalServerError(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var responseMessage = new
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = $"An unexpected error occurred: {ex.Message}"
            };
            await context.Response.WriteAsJsonAsync(responseMessage);
        }

        private static async Task HandleForbidden(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var responseMessage = new
            {
                Status = StatusCodes.Status403Forbidden,
                Message = "Access denied."
            };

            await context.Response.WriteAsJsonAsync(responseMessage);
        }

        private static async Task HandleUnauthorized(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var responseMessage = new
            {
                Status = StatusCodes.Status401Unauthorized,
                Message = "login to continue!"
            };

            await context.Response.WriteAsJsonAsync(responseMessage);
        }

        private static async Task HandleNotFound(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var responseMessage = new
            {
                Status = StatusCodes.Status404NotFound,
                Message = $"Requested path is not found: {context.Request.Path}"
            };

            await context.Response.WriteAsJsonAsync(responseMessage);
        }
    }
}
