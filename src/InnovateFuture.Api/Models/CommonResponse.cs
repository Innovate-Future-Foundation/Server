using System.Text.Json.Serialization;
using InnovateFuture.Application.Common.Models;

namespace InnovateFuture.Api.Models;
public class CommonResponse<T>
{
    /*
     * 1. Success: Used to indicate whether the operation was successful.
       2. Message: The message describing the success or failure of the operation.
       3. Data: The specific data returned, which can be empty.
       4. Errors: A list of error messages returned when the operation fails.
       5. Meta: Additional metadata about the operation, often used for pagination or other auxiliary information.
     */
    public bool IsSuccess { get; set; } = true;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Errors { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Meta? Meta { get; set; }
    
}
