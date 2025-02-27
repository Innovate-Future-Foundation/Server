namespace InnovateFuture.Api.Controllers.ToursController;

public class CreateTourRequest
{
    public Guid OrgId { get; set; }
    public string Title { get; set; }
    public string? Comment { get; set; }
    public string? Summary { get; set; }
    public string? Text { get; set; }
    public string? CoverImgUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? Leader { get; set; }
}