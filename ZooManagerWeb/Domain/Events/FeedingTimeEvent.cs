namespace ZooManagerWeb.Domain.Events;

public class FeedingTimeEvent
{
    public Guid FeedingScheduleId { get; }
    public Guid AnimalId { get; }
    public string FoodType { get; }
    public DateTime ScheduledTime { get; }
    public DateTime TriggeredAt { get; }

    public FeedingTimeEvent(Guid feedingScheduleId, Guid animalId, string foodType, DateTime scheduledTime)
    {
        FeedingScheduleId = feedingScheduleId;
        AnimalId = animalId;
        FoodType = foodType;
        ScheduledTime = scheduledTime;
        TriggeredAt = DateTime.UtcNow;
    }
}