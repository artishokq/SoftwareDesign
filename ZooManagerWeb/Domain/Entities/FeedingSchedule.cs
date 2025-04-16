namespace ZooManagerWeb.Domain.Entities;

public class FeedingSchedule
{
    public Guid Id { get; }
    public Guid AnimalId { get; }
    public string FoodType { get; private set; }
    public DateTime FeedingTime { get; private set; }
    public bool FeedingStatus { get; private set; }

    public FeedingSchedule(Guid animalId, string foodType, DateTime feedingTime)
    {
        Id = Guid.NewGuid();
        AnimalId = animalId;
        FoodType = foodType;
        FeedingTime = feedingTime;
        FeedingStatus = false;
    }

    // Метод для изменения расписания
    public void UpdateSchedule(string newFoodType, DateTime newFeedingTime)
    {
        FoodType = newFoodType;
        FeedingTime = newFeedingTime;
    }

    // Метод для отметки о выполнении кормления
    public void isCompleted()
    {
        FeedingStatus = true;
    }
    
    public void ResetFeedingStatus()
    {
        FeedingStatus = false;
    }
}