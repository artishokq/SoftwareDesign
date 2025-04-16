namespace ZooManagerWebTests.ApplicationTests;

using Xunit;
using ZooManagerWeb.Application.Services;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Animal;
using ZooManagerWeb.Infrastructure.Repositories;

public class FeedingOrganizationServiceTests
{
    private readonly InMemoryAnimalRepository _animalRepository;
    private readonly InMemoryFeedingScheduleRepository _feedingScheduleRepository;
    private readonly FeedingOrganizationService _service;

    public FeedingOrganizationServiceTests()
    {
        _animalRepository = new InMemoryAnimalRepository();
        _feedingScheduleRepository = new InMemoryFeedingScheduleRepository();
        _service = new FeedingOrganizationService(_animalRepository, _feedingScheduleRepository);
    }

    [Fact]
    public async Task CreateFeedingScheduleAsync_ShouldCreateAndReturnFeedingSchedule()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);

        string foodType = "Мясо";
        DateTime feedingTime = DateTime.UtcNow.AddHours(1);

        // Act
        var result = await _service.CreateFeedingScheduleAsync(animal.Id, foodType, feedingTime);
        
        var savedSchedules = await _feedingScheduleRepository.GetAllAsync();
        var savedSchedule = savedSchedules.FirstOrDefault();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(animal.Id, result.AnimalId);
        Assert.Equal(foodType, result.FoodType);
        Assert.Equal(feedingTime, result.FeedingTime);
        Assert.False(result.FeedingStatus);

        Assert.NotNull(savedSchedule);
        Assert.Equal(result.Id, savedSchedule.Id);
    }

    [Fact]
    public async Task CreateFeedingScheduleAsync_ShouldThrowException_WhenAnimalNotFound()
    {
        // Arrange
        var nonExistentAnimalId = Guid.NewGuid();
        string foodType = "Мясо";
        DateTime feedingTime = DateTime.UtcNow.AddHours(1);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateFeedingScheduleAsync(nonExistentAnimalId, foodType, feedingTime));
    }

    [Fact]
    public async Task UpdateFeedingScheduleAsync_ShouldUpdateSchedule()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);

        var schedule = new FeedingSchedule(animal.Id, "Мясо", DateTime.UtcNow);
        await _feedingScheduleRepository.AddAsync(schedule);

        string newFoodType = "Овощи";
        DateTime newFeedingTime = DateTime.UtcNow.AddHours(2);

        // Act
        await _service.UpdateFeedingScheduleAsync(schedule.Id, newFoodType, newFeedingTime);
        var updatedSchedule = await _feedingScheduleRepository.GetByIdAsync(schedule.Id);

        // Assert
        Assert.NotNull(updatedSchedule);
        Assert.Equal(newFoodType, updatedSchedule.FoodType);
        Assert.Equal(newFeedingTime, updatedSchedule.FeedingTime);
    }

    [Fact]
    public async Task MarkFeedingAsCompletedAsync_ShouldMarkFeedingAsCompleted()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);
        var schedule = new FeedingSchedule(animal.Id, "Мясо", DateTime.UtcNow);
        await _feedingScheduleRepository.AddAsync(schedule);

        // Act
        await _service.MarkFeedingAsCompletedAsync(schedule.Id);
        var updatedSchedule = await _feedingScheduleRepository.GetByIdAsync(schedule.Id);

        // Assert
        Assert.NotNull(updatedSchedule);
        Assert.True(updatedSchedule.FeedingStatus);
    }

    [Fact]
    public async Task CheckUpcomingFeedingsAsync_ShouldRaiseEvents_ForUpcomingFeedings()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);
        
        var schedule = new FeedingSchedule(
            animal.Id,
            "Meat",
            DateTime.UtcNow.AddMinutes(10)
        );
        await _feedingScheduleRepository.AddAsync(schedule);

        int eventCount = 0;
        _service.FeedingTime += (e) => eventCount++;

        // Act
        await _service.CheckUpcomingFeedingsAsync();

        // Assert
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public async Task CheckUpcomingFeedingsAsync_ShouldNotRaiseEvents_ForCompletedFeedings()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);
        
        var schedule = new FeedingSchedule(
            animal.Id,
            "Мясо",
            DateTime.UtcNow.AddMinutes(10)
        );
        await _feedingScheduleRepository.AddAsync(schedule);
        await _service.MarkFeedingAsCompletedAsync(schedule.Id);

        int eventCount = 0;
        _service.FeedingTime += (e) => eventCount++;

        // Act
        await _service.CheckUpcomingFeedingsAsync();

        // Assert
        Assert.Equal(0, eventCount);
    }
}