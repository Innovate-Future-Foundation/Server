namespace InnovateFuture.Domain.Entities;
public class ActivityDay
{
    public Guid ActivitiesId { get; private set; }
    public Activity Activity { get; private set; }
    public Guid DaysBelongId { get; private set; }
    public Day Day { get; private set; }
    public ActivityDay()
    {
    }
    public ActivityDay(Activity activity,  Day day)
    {
        ActivitiesId = activity.Id;
        DaysBelongId= day.Id;
    }
}