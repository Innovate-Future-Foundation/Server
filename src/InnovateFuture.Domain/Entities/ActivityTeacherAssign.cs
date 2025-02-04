namespace InnovateFuture.Domain.Entities;

public class ActivityTeacherAssign
{
    public long ProfileId { get; private set; }
    public long TourId { get; private set; }

    public ActivityTeacherAssign(long profileId, long tourId)
    {
        ProfileId = profileId;
        TourId = tourId;
    }
}