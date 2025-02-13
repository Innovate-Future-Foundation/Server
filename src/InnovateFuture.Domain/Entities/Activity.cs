using System.ComponentModel.DataAnnotations;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Activity
{
    public Guid Id { get; private set; }
    public Guid OrgId { get; private set; }
    public Guid DayId { get; private set; }
    [MaxLength(50)]
    public string Title { get; private set; }
    [MaxLength(255)]
    public string? Description { get; private set; }
    [MaxLength(100)]
    public string? Location { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    [MaxLength(500)]
    public string? CoverImgUrl { get; private set; }
    public TourStatusEnum Status { get; private set; }
    public DateTime CreateAt { get; private set; }
    public DateTime UpdateAt { get; private set; }
    public Day Day { get; private set; }

    public ICollection<Profile>? TeachersAssigned { get; private set; } = new List<Profile>();
    
    public Activity(){}

    public Activity(
        Guid orgId, 
        Guid dayId, 
        string title, 
        DateTime startTime, 
        DateTime endTime, 
        Guid? id=null, 
        string? description=null, 
        string? coverImgUrl=null,
        string? location=null
        )
    {
        Id = id?? Guid.NewGuid();
        OrgId = orgId;
        DayId = dayId;
        Title = title;
        Description = description;
        Status = TourStatusEnum.Draft;
        CoverImgUrl = coverImgUrl;
        StartTime = startTime == default ? DateTime.UtcNow : startTime;
        EndTime = endTime == default ? DateTime.UtcNow : endTime;
        Location = location;
        CreateAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }
    public void UpdateActivity(
        string? title, 
        DateTime? startTime, 
        DateTime? endTime, 
        string? description, 
        string? coverImgUrl,
        string? location,
        TourStatusEnum? status
        )
    {
        Title = string.IsNullOrWhiteSpace(title)?Title:title;
        StartTime = startTime??StartTime;
        EndTime = endTime??EndTime;
        Description = string.IsNullOrWhiteSpace(description)?Description:description;
        CoverImgUrl = string.IsNullOrWhiteSpace(coverImgUrl)?CoverImgUrl:coverImgUrl;
        Location = string.IsNullOrWhiteSpace(location)?Location:location;
        Status = status ?? TourStatusEnum.Draft;
        UpdateAt = DateTime.UtcNow;
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
        Day = day??throw new ArgumentNullException(nameof(day), "Day cannot be null.");
        DayId = day.Id;
    }
}