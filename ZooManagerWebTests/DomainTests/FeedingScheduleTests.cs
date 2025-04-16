namespace ZooManagerWebTests.DomainTests;

using Xunit;
using ZooManagerWeb.Domain.Entities;

public class FeedingScheduleTests
{
    [Fact]
    public void Constructor_ShouldCreateFeedingSchedule_WithCorrectProperties()
    {
        // Arrange
        var animalId = Guid.NewGuid();
        string foodType = "Мясо";
        DateTime feedingTime = DateTime.UtcNow.AddHours(1);

        // Act
        var schedule = new FeedingSchedule(animalId, foodType, feedingTime);

        // Assert
        Assert.Equal(animalId, schedule.AnimalId);
        Assert.Equal(foodType, schedule.FoodType);
        Assert.Equal(feedingTime, schedule.FeedingTime);
        Assert.False(schedule.FeedingStatus);
        Assert.NotEqual(Guid.Empty, schedule.Id);
    }

    [Fact]
    public void UpdateSchedule_ShouldUpdateFoodTypeAndFeedingTime()
    {
        // Arrange
        var schedule = new FeedingSchedule(Guid.NewGuid(), "Мясо", DateTime.UtcNow);
        string newFoodType = "Овощи";
        DateTime newFeedingTime = DateTime.UtcNow.AddHours(2);

        // Act
        schedule.UpdateSchedule(newFoodType, newFeedingTime);

        // Assert
        Assert.Equal(newFoodType, schedule.FoodType);
        Assert.Equal(newFeedingTime, schedule.FeedingTime);
    }

    [Fact]
    public void isCompleted_ShouldSetFeedingStatusToTrue()
    {
        // Arrange
        var schedule = new FeedingSchedule(Guid.NewGuid(), "Мясо", DateTime.UtcNow);

        // Act
        schedule.isCompleted();

        // Assert
        Assert.True(schedule.FeedingStatus);
    }

    [Fact]
    public void ResetFeedingStatus_ShouldSetFeedingStatusToFalse()
    {
        // Arrange
        var schedule = new FeedingSchedule(Guid.NewGuid(), "Мясо", DateTime.UtcNow);
        schedule.isCompleted();

        // Act
        schedule.ResetFeedingStatus();

        // Assert
        Assert.False(schedule.FeedingStatus);
    }
}