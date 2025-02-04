using InnovateFuture.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Domain.Entities;

public class Tour
{
    public Guid TourId { get; private set; }
    public Guid OrgId { get; private set; }
    public Organisation? Organisation { get; private set; }
    public Guid TourTempId { get; private set; }
    public TourTemplate? TourTemplate { get; private set; }

    // FK of tour is Profile entity ProfileId
    public Guid? TourLeadId { get; private set; }
    public Profile? Profile { get; private set; }
    public string Title { get; private set; }
    public string Text { get; private set; }
    public string Summary { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public ICollection<Day>? Days { get; private set; } = new List<Day>();
    public StatusEnum Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Tour() { }

    public Tour(
     Guid orgId,
     Guid templateId,
     string? title,
     string? text,
     string? summary,
     Guid? tourLeadId,
     DateTime startDate,
     DateTime endDate
 )
    {
        OrgId = orgId;
        TourTempId = templateId;
        Title = title;
        Text = text;
        Summary = summary;
        Status = StatusEnum.Pending; // initial status
        TourLeadId = tourLeadId;
        StartDate = startDate;
        EndDate = endDate;
        CreatedAt = DateTime.UtcNow;
    }


    public void UpdateDates(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("Start date must be earlier than end date.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }



    public void UpdateTourDetails
        (
        string title,
        string text,
        string summary,
        StatusEnum status
        )
    {
        Title = string.IsNullOrWhiteSpace(title) ? Title : title;
        Text = string.IsNullOrWhiteSpace(text) ? Text : text;
        Summary = string.IsNullOrWhiteSpace(summary) ? Summary : summary;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    //  assign Tour lead
    public void AssignTourLead(Guid tourLeadId)
    {
        TourLeadId = tourLeadId;
    }

    // Add Day
    public void AddDay(Day day)
    {
        if (day == null)
        {
            throw new ArgumentNullException(nameof(day), "Day cannot be null.");
        }
        Days.Add(day);
    }
}

