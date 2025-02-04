namespace InnovateFuture.Api.Controllers.ActivityController
{
    public class CreateActivityRequest
    {
        public long DayId { get; set; }
        public long TemplateId { get; set; }
        public long ActivityLead { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Summary { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}