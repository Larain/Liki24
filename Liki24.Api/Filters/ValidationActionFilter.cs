using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Liki24.Api.Filters
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                // log response, invalid objects.. etc.
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}