using System.ComponentModel.DataAnnotations;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Activity
{
    public Guid Id { get; private set; }
    public Guid OrgId { get; private set; }
    public string Title { get; private set; }
    public string? Comment { get; private set; }
    public string? Summary { get; private set; }
    public string? Text { get; private set; }
    public string? Location { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string? CoverImgUrl { get; private set; }
    public TourStatusEnum Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Organisation Organisation { get; private set; }
    public ICollection<Day> DaysBelong { get; private set; } = new List<Day>();
    public ICollection<Profile>? TeachersAssigned { get; private set; } = new List<Profile>();
    
    public Activity(){}

    public Activity(
        Guid orgId, 
        string title,
        DateTime startTime, 
        DateTime endTime, 
        Guid? id = null, 
        string? comment = null, 
        string? summary = null,
        string? text = null,
        string? coverImgUrl = null,
        string? location = null
    )
    {
        Id = id ?? Guid.NewGuid();
        OrgId = orgId;
        Title = title;
        Comment = string.IsNullOrWhiteSpace(comment) ? null : comment;
        Summary = string.IsNullOrWhiteSpace(summary) ? null : summary;
        Text = string.IsNullOrWhiteSpace(text) ? null : text;
        Status = TourStatusEnum.Draft;
        CoverImgUrl = string.IsNullOrWhiteSpace(coverImgUrl) ? null : coverImgUrl;
        StartTime = startTime == default ? DateTime.UtcNow : startTime;
        EndTime = endTime == default ? DateTime.UtcNow : endTime;
        Location = string.IsNullOrWhiteSpace(location) ? null : location;
        CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }
    public void UpdateActivity(
        string? title, 
        DateTime? startTime, 
        DateTime? endTime, 
        string? comment,
        string? summary,
        string? text, 
        string? coverImgUrl,
        string? location,
        TourStatusEnum? status
        )
    {
        Title = string.IsNullOrWhiteSpace(title)?Title:title;
        StartTime = startTime??StartTime;
        EndTime = endTime??EndTime;
        Comment = string.IsNullOrWhiteSpace(comment)?Comment:comment;
        Summary = string.IsNullOrWhiteSpace(summary)?Summary:summary;
        Text = string.IsNullOrWhiteSpace(text)?Text:text;
        CoverImgUrl = string.IsNullOrWhiteSpace(coverImgUrl)?CoverImgUrl:coverImgUrl;
        Location = string.IsNullOrWhiteSpace(location)?Location:location;
        Status = status ?? TourStatusEnum.Draft;
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }

    public void AssignTeacher(Profile teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher), "Teacher cannot be null.");
        }
        TeachersAssigned?.Add(teacher);
    }
    public void RemoveTeacher(Profile teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher), "Teacher cannot be null.");
        }
        TeachersAssigned?.Remove(teacher);
    }

    public void AddDay(Day day)
    {
        if (day == null)
        {
            throw new ArgumentNullException(nameof(day), "Day cannot be null.");
        }
        DaysBelong?.Add(day);
    }
    public void RemoveDay(Day day)
    {
        if (day == null)
        {
            throw new ArgumentNullException(nameof(day), "Day cannot be null.");
        }
        DaysBelong?.Remove(day);
    }
    public void AddOrganisation(Organisation organisation)
    {
        Organisation = organisation?? throw new ArgumentNullException(nameof(organisation), "Organisation cannot be null.");
        OrgId = organisation.Id;
    }
}