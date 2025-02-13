using System.ComponentModel.DataAnnotations;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Tour
{
    public Guid Id { get; private set; }
    public Guid OrgId { get; private set; }
    [MaxLength(50)]
    public string Title { get; private set; }
    [MaxLength(255)]
    public string? Description { get; private set; }

    [MaxLength(500)]
    public string? CoverImgUrl { get; private set; }
    public DateTime StartDate{ get; private set; }
    public DateTime EndDate { get; private set; }
    public TourStatusEnum Status { get; private set; }
    public Guid? Leader { get; private set; }
    public Profile? LeaderProfile { get; private set; }
    public ICollection<Day>? Days { get; private set; }=new List<Day>();
    public DateTime CreateAt { get; private set; }
    public DateTime UpdateAt { get; private set; }
    
    public Tour(){}

    public Tour(
        Guid orgId,
        string title,
        DateTime startDate,
        DateTime endDate,
        Guid? leader=null,
        Guid? id=null, 
        string? description=null, 
        string? coverImgUrl=null
        )
    {
        Id = id?? Guid.NewGuid();
        OrgId = orgId;
        Title = title;
        Description = description;
        Status = TourStatusEnum.Draft;
        CoverImgUrl = coverImgUrl;
        StartDate= StartDate == default ? DateTime.UtcNow : startDate;
        EndDate= EndDate == default ? DateTime.UtcNow: endDate;
        Leader = leader;
        CreateAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }
    public void UpdateTour(
        string? title, 
        string? description, 
        string? coverImgUrl,
        DateTime? startDate,
        DateTime? endDate,
        Guid? leader,
        TourStatusEnum? status
        )
    {
        Title = string.IsNullOrWhiteSpace(title)?Title:title;
        Description = string.IsNullOrWhiteSpace(description)?Description:description;
        CoverImgUrl = string.IsNullOrWhiteSpace(coverImgUrl)?CoverImgUrl:coverImgUrl;
        Status = status ?? TourStatusEnum.Draft;
        Leader = leader??leader;
        StartDate = startDate??StartDate;
        EndDate = endDate??EndDate;
        UpdateAt = DateTime.UtcNow;
    }

    public void AssignLeader(Profile teacher)
    {
        LeaderProfile = teacher?? throw new ArgumentNullException(nameof(teacher));
        Leader = teacher.Id;
    }

    public void AddDay(Day day)
    {
        if (day == null)
        {
            throw new ArgumentNullException(nameof(day), "Day cannot be null.");
        }
        Days?.Add(day);
    }

    public void RemoveDay(Day day)
    {
        if(day == null)
        {
            throw new ArgumentNullException(nameof(day), "Day cannot be null.");
        }
        Days?.Remove(day);
    }
}