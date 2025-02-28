namespace InnovateFuture.Domain.Entities;

public class ActivityProfile
{
    public Guid AssignedActivitiesId { get; private set; }
    public Activity Activity { get; private set; }
    public Guid TeachersAssignedId { get; private set; }
    public Profile Teacher { get; private set; }
    
    public ActivityProfile()
    {
    }
    public ActivityProfile(Activity activity,  Profile Teacher)
    {
        AssignedActivitiesId = activity.Id;
        TeachersAssignedId= Teacher.Id;
    }
}