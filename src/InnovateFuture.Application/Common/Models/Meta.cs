using System.Text.Json.Serialization;

namespace InnovateFuture.Application.Common.Models;

public class Meta
{
    public int TotalItems { get; set; }
    // Cursor-based pagination
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NextCursor { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HasNext { get; set; }
}