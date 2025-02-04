using System;

namespace InnovateFuture.Domain.Entities
{
    public class ActivityTemplate
    {
        public Guid ActivityTempId { get; protected set; } // 主键，允许 EF Core 访问
        public Guid? DayTempId { get; private set; } // 外键
        public DayTemplate? DayTemplate { get; private set; } // 导航属性
        public string? Title { get; private set; }
        public string? Text { get; private set; }
        public string? Summary { get; private set; }

        // For EF Core
        public ActivityTemplate() { }

        // Constructor with parameters
        public ActivityTemplate(Guid dayTempId, string title, string text, string summary)
        {
            ActivityTempId = Guid.NewGuid();
            DayTempId = dayTempId;
            Title = title;
            Text = text;
            Summary = summary;
        }

        // Update method
        public void UpdateDetails(string title, string text, string summary)
        {
            Title = string.IsNullOrWhiteSpace(title) ? Title : title;
            Text = string.IsNullOrWhiteSpace(text) ? Text : text;
            Summary = string.IsNullOrWhiteSpace(summary) ? Summary : summary;
        }
    }
}
