using InnovateFuture.Api.Models;
using InnovateFuture.Application.Common.Models;
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
                object? data = null;
                object? meta = null;
                var resultType = resultValue.GetType();
                
                if ( resultType.IsGenericType&&resultType.GetGenericTypeDefinition() == typeof(PaginatedResult<>))
                {
                    // Extract Data and MetaData from PaginatedResult<>
                    data = resultType.GetProperty("Data")?.GetValue(resultValue);
                    meta = resultType.GetProperty("Meta")?.GetValue(resultValue);
                }
                else
                {
                    // For non-paginated results, just use the value directly as data
                    data = resultValue;
                }
                
                // confirm T of CommonResponse<T> 
                var commonResponseType = typeof(CommonResponse<>).MakeGenericType(data?.GetType() ?? typeof(object));

                // create CommonResponse<T> instance
                var commonResponse = Activator.CreateInstance(commonResponseType);

                commonResponseType.GetProperty("Data")?.SetValue(commonResponse, data);
                
                commonResponseType.GetProperty("Meta")?.SetValue(commonResponse, meta);

                commonResponseType.GetProperty("Message")?.SetValue(commonResponse, $"Success - Timestamp: {DateTime.UtcNow}");

                objectResult.Value = commonResponse;
            }
        }
    }
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}