namespace InnovateFuture.Domain.Entities;

public class ActivityFilterModel
{
    
    public long? OrgId { get; set; }
    public long? DayId { get; set; }
    public long? TemplateId { get; set; }
    public long? ActivityLead { get; set; }
    public string Title { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Status { get; set; }

    // 添加验证方法
    public bool IsValid()
    {
        // 如果设置了开始时间和结束时间,确保开始时间早于结束时间
        if (StartTime.HasValue && EndTime.HasValue)
        {
            return StartTime.Value < EndTime.Value;
        }
        return true;
    }
}
