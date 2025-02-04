using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Domain.Entities;

public class DayTemplate
{
    public Guid DayTempId { get; private set; }
    public Guid? TourTempId { get; private set; }
    public TourTemplate? TourTemplate { get; private set; }
    public string? Title { get; private set; }
    public string? Text { get; private set; }
    public string? Summary { get; private set; }
    public ICollection<ActivityTemplate>? ActivityTemplates { get; private set; } = new List<ActivityTemplate>();

    public ICollection<Day>? Days { get; private set; } = new List<Day>();


    // For EF Core 
    public DayTemplate() { }

    // Constructor
    public DayTemplate(Guid tourTempId, string title, string text, string summary)
    {
        DayTempId = Guid.NewGuid();
        TourTempId = tourTempId;
        Title = title;
        Text = text;
        Summary = summary;
    }

    // Add ActivityTemplate
    public void AddActivityTemplate(ActivityTemplate activityTemplate)
    {
        if (activityTemplate == null)
        {
            throw new ArgumentNullException(nameof(activityTemplate), "ActivityTemplate cannot be null.");
        }
        ActivityTemplates.Add(activityTemplate);
    }

}
