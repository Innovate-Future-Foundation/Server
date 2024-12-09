using InnovateFuture.Api.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InnovateFuture.Api.Filters;

public class CommonResultFilter:IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            var resultValue = objectResult.Value;
            if (resultValue is not CommonResponse<object>)
            {
                // confirm T of CommonResponse<T> 
                var commonResponseType = typeof(CommonResponse<>).MakeGenericType(resultValue.GetType());

                // create CommonResponse<T> instance
                var commonResponse = Activator.CreateInstance(commonResponseType);

                commonResponseType.GetProperty("Data")?.SetValue(commonResponse, resultValue);

                commonResponseType.GetProperty("Message")?.SetValue(commonResponse, $"Success - Timestamp: {DateTime.UtcNow}");

                objectResult.Value = commonResponse;
            }
        }
    }
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}