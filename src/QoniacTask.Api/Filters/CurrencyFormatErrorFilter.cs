using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace QoniacTask.Api.Filters
{
    public class CurrencyFormatErrorFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is FormatException fex)
            {
                var problemDetailsFactory =
                            context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var validationProblemDetails =
                    problemDetailsFactory.CreateValidationProblemDetails(
                        context.HttpContext, context.ModelState);

                validationProblemDetails.Type =
                    "https://datatracker.ietf.org/doc/html/rfc9110#name-422-unprocessable-content";
                validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                validationProblemDetails.Detail = fex.Message;

                context.Result = new UnprocessableEntityObjectResult(validationProblemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };

                context.ExceptionHandled = true;
            }
        }
    }
}