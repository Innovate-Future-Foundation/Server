using InnovateFuture.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InnovateFuture.Api.Filters;

    public class ModelValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new CommonResponse<object>
                {
                    IsSuccess = false,
                    Errors = errors
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
