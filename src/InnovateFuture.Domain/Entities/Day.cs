using System.ComponentModel.DataAnnotations;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Day
{
    public Guid Id { get; private set; }
    public Guid OrgId { get; private set; }
    public Guid TourId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public string? CoverImgUrl { get; private set; }
    public TourStatusEnum Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Organisation Organisation { get; private set; }
    
    public Tour Tour { get; private set; }
    public ICollection<Activity>? Activities { get; private set; }=new List<Activity>();
    public Day(){}

    public Day(
        Guid orgId, 
        Guid tourId, 
        string title, 
        Guid? id=null, 
        string? description=null, 
        string? coverImgUrl=null
        )
    {
        Id = id?? Guid.NewGuid();
        OrgId = orgId;
        TourId = tourId;
        Title = title;
        Description = description;
        Status = TourStatusEnum.Draft;
        CoverImgUrl = coverImgUrl;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateDay(
        string? title, 
        string? description, 
        string? coverImgUrl,
        TourStatusEnum? status
        )
    {
        Title = string.IsNullOrWhiteSpace(title)?Title:title;
        Description = string.IsNullOrWhiteSpace(description)?Description:description;
        CoverImgUrl = string.IsNullOrWhiteSpace(coverImgUrl)?CoverImgUrl:coverImgUrl;
        Status = status ?? TourStatusEnum.Draft;
        UpdatedAt = DateTime.UtcNow;
    }
    public void AddActivity(Activity activity)
    {
        if (activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        Activities?.Add(activity);
    }

    public void RemoveActivity(Activity activity)
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