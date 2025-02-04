namespace InnovateFuture.Domain.Entities;

public class ActivityTemplate
{
    public long ActivityTempId { get; private set; }
    public long DayTempId { get; private set; }
    public long OrgId { get; private set; }
    public string Title { get; private set; }
    public string Text { get; private set; }
    public string Summary { get; private set; }

    public ActivityTemplate(
        long dayTempId, 
        long orgId, 
        string title, 
        string text, 
        string summary)
    {
        DayTempId = dayTempId;
        OrgId = orgId;
        Title = title;
        Text = text;
        Summary = summary;
    }
}