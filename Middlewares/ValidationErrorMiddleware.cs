using System.Net;
using System.Text.Json;
using TaskManager.Api.Models;

namespace TaskManager.Api.Middlewares
{
    public class ValidationErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest &&
                    context.Items.ContainsKey("ValidationErrors"))
            {
                context.Response.ContentType = "application/json";

                var errors = context.Items["ValidationErrors"];
                var response = ApiResponse<object>.Fail("Validation failed");
                response.Data = errors;

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
