using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Domain.Entities;

public class Activity
{
    public Guid ActivityId { get; private set; } // key
    public Guid? DayId { get; private set; } // Foreigh Key
    public Day? Day { get; private set; } // Navigation property

    public Guid? ActivityTemplateId { get; private set; } // Foreign key ActivityTemplate
    public ActivityTemplate? ActivityTemplate { get; private set; }// Navigation property ActivityTemplate

    public string? Title { get; private set; } // title
    public string? Text { get; private set; } // description
    public string? Summary { get; private set; } // summary
    public DateTime StartTime { get; private set; } 
    public DateTime EndTime { get; private set; } 

    // For EF Core 
    public Activity() { }

    // Constructor parameter
    public Activity(Guid dayId, Guid? activityTemplateId, string title, string text, string summary, DateTime startTime, DateTime endTime)
    {
        ActivityId = Guid.NewGuid();
        DayId = dayId;
        ActivityTemplateId = activityTemplateId;
        Title = title;
        Text = text;
        Summary = summary;
        StartTime = startTime;
        EndTime = endTime;
    }

    // Update Activity
    public void UpdateDetails(string title, string text, string summary, DateTime startTime, DateTime endTime)
    {
        Title = string.IsNullOrWhiteSpace(title) ? Title : title;
        Text = string.IsNullOrWhiteSpace(text) ? Text : text;
        Summary = string.IsNullOrWhiteSpace(summary) ? Summary : summary;
        StartTime = startTime == default ? StartTime : startTime;
        EndTime = endTime == default ? EndTime : endTime;
    }
}
