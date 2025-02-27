using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Api.Controllers.ActivitiesController;

public class GetActivityResponse
{
    public Guid Id { get; set; }
    public Guid OrgId { get; set; }
    public string Title { get; set; }
    public string? Comment { get; set; }
    public string? Summary { get; set; }
    public string? Text { get; set; }
    public string? Location { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? CoverImgUrl { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}