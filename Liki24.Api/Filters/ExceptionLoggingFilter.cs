using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Liki24.Api.Filters
{
    public class ExceptionLoggingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //log url, arguments, exception, etc..
            context.Result = new StatusCodeResult(500);
            context.ExceptionHandled = true;
            base.OnException(context);
        }
    }
}