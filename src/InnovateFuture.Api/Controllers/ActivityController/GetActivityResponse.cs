using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Controllers.ActivityController
{
    public class GetActivityResponse
    {
        public long ActivityId { get; set; }
        public long OrgId { get; set; }
        public long DayId { get; set; }
        public long TemplateId { get; set; }
        public long ActivityLead { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Summary { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }

        public GetActivityResponse(DomainActivity domainActivity)
        {
            ActivityId = domainActivity.ActivityId;
            OrgId = domainActivity.OrgId;
            DayId = domainActivity.DayId;
            TemplateId = domainActivity.TemplateId;
            ActivityLead = domainActivity.ActivityLead;
            Title = domainActivity.Title;
            Text = domainActivity.Text;
            Summary = domainActivity.Summary;
            StartTime = domainActivity.StartTime;
            EndTime = domainActivity.EndTime;
            Status = domainActivity.Status;
        }
    }
}