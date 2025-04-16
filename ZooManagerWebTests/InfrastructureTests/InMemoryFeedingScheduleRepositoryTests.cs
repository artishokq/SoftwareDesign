namespace ZooManagerWebTests.InfrastructureTests;

using Xunit;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Infrastructure.Repositories;

public class InMemoryFeedingScheduleRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenRepositoryIsEmpty()
    {
        // Arrange
        var repository = new InMemoryFeedingScheduleRepository();

        // Act
        var schedules = await repository.GetAllAsync();

        // Assert
        Assert.Empty(schedules);
    }

    [Fact]
    public async Task AddAsync_ShouldAddSchedule_ToRepository()
    {
        // Arrange
        var repository = new InMemoryFeedingScheduleRepository();
        var animalId = Guid.NewGuid();
        var schedule = new FeedingSchedule(animalId, "Мясо", DateTime.Now);

        // Act
        await repository.AddAsync(schedule);
        var schedules = await repository.GetAllAsync();

        // Assert
        Assert.Single(schedules);
        Assert.Same(schedule, schedules.First());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSchedule_WhenExists()
    {
        // Arrange
        var repository = new InMemoryFeedingScheduleRepository();
        var animalId = Guid.NewGuid();
        var schedule = new FeedingSchedule(animalId, "Мясо", DateTime.Now);
        await repository.AddAsync(schedule);

        // Act
        var result = await repository.GetByIdAsync(schedule.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Same(schedule, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        var repository = new InMemoryFeedingScheduleRepository();

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByAnimalIdAsync_ShouldReturnSchedules_ForAnimal()
    {
        // Arrange
        var repository = new InMemoryFeedingScheduleRepository();

        var animalId1 = Guid.NewGuid();
        var animalId2 = Guid.NewGuid();

        var schedule1 = new FeedingSchedule(animalId1, "Мясо", DateTime.Now);
        var schedule2 = new FeedingSchedule(animalId1, "Мясо", DateTime.Now.AddHours(6));
        var schedule3 = new FeedingSchedule(animalId2, "Овощи", DateTime.Now);

        await repository.AddAsync(schedule1);
        await repository.AddAsync(schedule2);
        await repository.AddAsync(schedule3);

        // Act
        var result = await repository.GetByAnimalIdAsync(animalId1);
        var schedules = result.ToList();

        // Assert
        Assert.Equal(2, schedules.Count);
        Assert.Contains(schedule1, schedules);
        Assert.Contains(schedule2, schedules);
        Assert.DoesNotContain(schedule3, schedules);
    }

    [Fact]
    public async Task GetUpcomingSchedulesAsync_ShouldReturnPendingSchedules_InTimeRange()
    {
        // Arrange
        var repository = new InMemoryFeedingScheduleRepository();
        var animalId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var fromTime = now;
        var toTime = now.AddHours(3);
        var schedule1 = new FeedingSchedule(animalId, "Мясо", now.AddHours(1));
        var schedule2 = new FeedingSchedule(animalId, "Мясо", now.AddHours(2));
        schedule2.isCompleted();
        var schedule3 = new FeedingSchedule(animalId, "Мясо", now.AddHours(4));
        await repository.AddAsync(schedule1);
        await repository.AddAsync(schedule2);
        await repository.AddAsync(schedule3);

        // Act
        var result = await repository.GetUpcomingSchedulesAsync(fromTime, toTime);
        var schedules = result.ToList();

        // Assert
        Assert.Single(schedules);
        Assert.Contains(schedule1, schedules);
        Assert.DoesNotContain(schedule2, schedules);
        Assert.DoesNotContain(schedule3, schedules);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateSchedule_WhenExists()
    {
        // Arrange
        var repository = new InMemoryFeedingScheduleRepository();
        var animalId = Guid.NewGuid();
        var schedule = new FeedingSchedule(animalId, "Мясо", DateTime.Now);
        await repository.AddAsync(schedule);

        // Act
        schedule.isCompleted();
        await repository.UpdateAsync(schedule);
        var updatedSchedule = await repository.GetByIdAsync(schedule.Id);

        // Assert
        Assert.True(updatedSchedule.FeedingStatus);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveSchedule_WhenExists()
    {
        // Arrange
        var repository = new InMemoryFeedingScheduleRepository();
        var animalId = Guid.NewGuid();
        var schedule = new FeedingSchedule(animalId, "Мясо", DateTime.Now);
        await repository.AddAsync(schedule);

        // Act
        await repository.DeleteAsync(schedule.Id);
        var result = await repository.GetByIdAsync(schedule.Id);
        var allSchedules = await repository.GetAllAsync();

        // Assert
        Assert.Null(result);
        Assert.Empty(allSchedules);
    }
}