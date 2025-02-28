using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Day
{
    public Guid Id { get; private set; }
    public Guid OrgId { get; private set; }
    public Guid TourId { get; private set; }
    public string Title { get; private set; }
    public string? Comment { get; private set; }
    public string? Summary { get; private set; }
    public string? Text { get; private set; }
    public string? CoverImgUrl { get; private set; }
    public TourStatusEnum Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Organisation Organisation { get; private set; }
    
    public Tour Tour { get; private set; }
    public ICollection<ActivityDay>? Activities { get; private set; }=new List<ActivityDay>();
    public Day(){}

    public Day(
        Guid orgId, 
        Guid tourId, 
        string title, 
        Guid? id = null, 
        string? comment = null,
        string? summary = null,
        string? text = null,
        string? coverImgUrl = null
    )
    {
        Id = id ?? Guid.NewGuid();
        OrgId = orgId;
        TourId = tourId;
        Title = title;
        Comment = string.IsNullOrWhiteSpace(comment) ? null : comment;
        Summary = string.IsNullOrWhiteSpace(summary) ? null : summary;
        Text = string.IsNullOrWhiteSpace(text) ? null : text;
        Status = TourStatusEnum.Draft;
        CoverImgUrl = string.IsNullOrWhiteSpace(coverImgUrl) ? null : coverImgUrl;
        CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }
    public void UpdateDay(
        string? title, 
        string? comment,
        string? summary,
        string? text, 
        string? coverImgUrl,
        TourStatusEnum? status
        )
    {
        Title = string.IsNullOrWhiteSpace(title)?Title:title;
        Comment = string.IsNullOrWhiteSpace(comment)?Comment:comment;
        Summary = string.IsNullOrWhiteSpace(summary)?Summary:summary;
        Text = string.IsNullOrWhiteSpace(text)?Text:text;
        CoverImgUrl = string.IsNullOrWhiteSpace(coverImgUrl)?CoverImgUrl:coverImgUrl;
        Status = status ?? TourStatusEnum.Draft;
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }
    public void AddActivity(ActivityDay activity)
    {
        if (activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        Activities?.Add(activity);
    }

    public void RemoveActivity(ActivityDay activity)
    {
        if(activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        Activities?.Remove(activity);
    }

    public void AddOrganisation(Organisation organisation)
    {
        Organisation = organisation?? throw new ArgumentNullException(nameof(organisation), "Organisation cannot be null.");
        OrgId = organisation.Id;
    }
    public void AddTour(Tour tour)
    {
        Tour = tour?? throw new ArgumentNullException(nameof(tour), "Tour cannot be null.");
        TourId = tour.Id;
    }
}