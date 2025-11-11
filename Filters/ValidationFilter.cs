using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManager.Api.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                context.HttpContext.Items["ValidationErrors"] = errors;

                context.Result = new BadRequestResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context){}
    }
}
