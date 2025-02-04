using InnovateFuture.Domain.Entities;

public class DomainActivity
{
    
    private DomainActivity() { }
    
    
    public long ActivityId { get; private set; }
    public long OrgId { get; private set; }
    public long DayId { get; private set; }
    public long TemplateId { get; private set; }
    public long ActivityLead { get; private set; }
    public string Title { get; private set; }
    public string Text { get; private set; }
    public string Summary { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Status { get; private set; }

   
    // public Day Day { get; private set; }
    //这里还没有，所以报错。
    
    
    public ActivityTemplate Template { get; private set; }
    public Profile ActivityLeadProfile { get; private set; }
    public Organisation Organisation { get; private set; }

    // 使用枚举替代字符串来表示状态
    public enum ActivityStatus
    {
        Draft,
        Published,
        InProgress,
        Completed,
        Cancelled
    }

    // 添加验证和业务规则的构造函数
    public static DomainActivity Create(
        long orgId, 
        long dayId, 
        long templateId, 
        long activityLead,
        string title,
        string text,
        string summary,
        DateTime startTime,
        DateTime endTime)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time");

        var activity = new DomainActivity
        {
            OrgId = orgId,
            DayId = dayId,
            TemplateId = templateId,
            ActivityLead = activityLead,
            Title = title,
            Text = text,
            Summary = summary,
            StartTime = startTime,
            EndTime = endTime,
            Status = ActivityStatus.Draft.ToString()
        };

        return activity;
    }

    // 添加状态转换方法
    public void Publish()
    {
        if (Status != ActivityStatus.Draft.ToString())
            throw new InvalidOperationException("Only draft activities can be published");
            
        Status = ActivityStatus.Published.ToString();
    }

    public void Start()
    {
        if (Status != ActivityStatus.Published.ToString())
            throw new InvalidOperationException("Only published activities can be started");
            
        Status = ActivityStatus.InProgress.ToString();
    }

    // 添加更新方法，确保数据一致性
    public void Update(
        string title = null,
        string text = null,
        string summary = null,
        DateTime? startTime = null,
        DateTime? endTime = null)
    {
        if (!string.IsNullOrWhiteSpace(title))
            Title = title;
            
        if (!string.IsNullOrWhiteSpace(text))
            Text = text;
            
        if (!string.IsNullOrWhiteSpace(summary))
            Summary = summary;

        if (startTime.HasValue && endTime.HasValue)
        {
            if (startTime.Value >= endTime.Value)
                throw new ArgumentException("Start time must be before end time");
                
            StartTime = startTime.Value;
            EndTime = endTime.Value;
        }
        else if (startTime.HasValue)
        {
            if (startTime.Value >= EndTime)
                throw new ArgumentException("Start time must be before current end time");
                
            StartTime = startTime.Value;
        }
        else if (endTime.HasValue)
        {
            if (StartTime >= endTime.Value)
                throw new ArgumentException("Current start time must be before end time");
                
            EndTime = endTime.Value;
        }
    }
}