using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Domain.Entities;

    public class TourTemplate
    {
        public Guid TourTempId { get; private set; }
        public Guid OrgId { get; private set; }
        public Organisation? Organisation { get; private set; }
        public string? Title { get; private set; }
        public string? Text { get; private set; }
        public string? Summary { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public ICollection<Tour>? Tours { get; private set; } = new List<Tour>();

    // 无参构造函数供 EF Core 使用
    public TourTemplate() { }

    // 主构造函数
    public TourTemplate(Guid orgId, string title, string text, string summary)
    {
        TourTempId = Guid.NewGuid();
        OrgId = orgId;
        Title = title;
        Text = text;
        Summary = summary;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    // update tour_template
    public void UpdateDetails(string title, string text, string summary)
    {
        Title = string.IsNullOrWhiteSpace(title) ? Title : title;
        Text = string.IsNullOrWhiteSpace(text) ? Text : text;
        Summary = string.IsNullOrWhiteSpace(summary) ? Summary : summary;
        UpdatedAt = DateTime.UtcNow;
    }

    // Add Tour 
    public void AddTour(Tour tour)
    {
        if (tour == null)
        {
            throw new ArgumentNullException(nameof(tour), "Tour cannot be null.");
        }

        Tours.Add(tour);
    }
}

