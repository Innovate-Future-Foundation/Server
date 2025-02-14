using System.ComponentModel.DataAnnotations;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Tour
{
    public Guid Id { get; private set; }
    public Guid OrgId { get; private set; }
    public string Title { get; private set; }
    public string? Comment { get; private set; }
    public string? Summary { get; private set; }
    public string? Text { get; private set; }
    public string? CoverImgUrl { get; private set; }
    public DateTime StartDate{ get; private set; }
    public DateTime EndDate { get; private set; }
    public TourStatusEnum Status { get; private set; }
    public Guid? Leader { get; private set; }
    public Profile? LeaderProfile { get; private set; }
    public Organisation Organisation { get; private set; }
    public ICollection<Day>? Days { get; private set; }=new List<Day>();
    public ICollection<StudentTourEnrollment>? StudentTourEnrollments { get; private set; } = new List<StudentTourEnrollment>();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public Tour(){}

    public Tour(
        Guid orgId,
        string title,
        DateTime startDate,
        DateTime endDate,
        Guid? leader=null,
        Guid? id=null, 
        string? comment=null, 
        string? summary=null,
        string? text=null,
        string? coverImgUrl=null
        )
    {
        Id = id?? Guid.NewGuid();
        OrgId = orgId;
        Title = title;
        Comment = comment;
        Summary = summary;
        Text = text;
        Status = TourStatusEnum.Draft;
        CoverImgUrl = coverImgUrl;
        StartDate = startDate == default ? DateTime.UtcNow : startDate;
        EndDate = endDate == default ? DateTime.UtcNow : endDate;
        Leader = leader ?? Leader;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateTour(
        string? title, 
        string? comment,
        string? summary,
        string? text, 
        string? coverImgUrl,
        DateTime? startDate,
        DateTime? endDate,
        Guid? leader,
        TourStatusEnum? status
        )
    {
        Title = string.IsNullOrWhiteSpace(title)?Title:title;
        Comment = string.IsNullOrWhiteSpace(comment)?Comment:comment;
        Summary = string.IsNullOrWhiteSpace(summary)?Summary:summary;
        Text = string.IsNullOrWhiteSpace(text)?Text:text;
        CoverImgUrl = string.IsNullOrWhiteSpace(coverImgUrl)?CoverImgUrl:coverImgUrl;
        Status = status ?? TourStatusEnum.Draft;
        Leader = leader??leader;
        StartDate = startDate??StartDate;
        EndDate = endDate??EndDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStudentTourEnrollment(StudentTourEnrollment studentTourEnrollment)
    {
        if (studentTourEnrollment == null)
        {
            throw new ArgumentNullException(nameof(studentTourEnrollment),"studentTourEnrollment cannot be null.");
        }
        StudentTourEnrollments?.Add(studentTourEnrollment);
    }
    public void RemoveStudentTourEnrollment(StudentTourEnrollment studentTourEnrollment)
    {
        if (studentTourEnrollment == null)
        {
            throw new ArgumentNullException(nameof(studentTourEnrollment),"studentTourEnrollment cannot be null.");
        }
        StudentTourEnrollments?.Remove(studentTourEnrollment);
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
    public void AddOrganisation(Organisation organisation)
    {
        Organisation = organisation?? throw new ArgumentNullException(nameof(organisation), "Organisation cannot be null.");
        OrgId = organisation.Id;
    }
}