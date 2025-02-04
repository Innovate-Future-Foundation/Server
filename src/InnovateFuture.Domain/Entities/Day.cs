using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Domain.Entities;

public class Day
{
    public Guid DayId { get; private set; } // Key
    public Guid? TourId { get; private set; } // Foreign key Tour
    public Tour? Tour { get; private set; } // Navigation property Tour

    public Guid? DayTemplateId { get; private set; } // Foreign key DayTemplate
    public DayTemplate? DayTemplate { get; private set; } // Navigation property DayTemplate

    public string? Title { get; private set; } 
    public string? Text { get; private set; } 
    public string? Summary { get; private set; } 
    public ICollection<Activity>? Activities { get; private set; } = new List<Activity>();

    // For EF Core 
    public Day() { }

    // Constructor 
    public Day(Guid tourId, Guid? dayTemplateId, string title, string text, string summary)
    {
        DayId = Guid.NewGuid();
        TourId = tourId;
        DayTemplateId = dayTemplateId;
        Title = title;
        Text = text;
        Summary = summary;
    }

    // update Day
    public void UpdateDetails(string title, string text, string summary)
    {
        Title = string.IsNullOrWhiteSpace(title) ? Title : title;
        Text = string.IsNullOrWhiteSpace(text) ? Text : text;
        Summary = string.IsNullOrWhiteSpace(summary) ? Summary : summary;
    }

    // Add Activity
    public void AddActivity(Activity activity)
    {
        if (activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        Activities.Add(activity);
    }
}
