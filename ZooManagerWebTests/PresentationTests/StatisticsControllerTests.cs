namespace ZooManagerWebTests.PresentationTests;

using Microsoft.AspNetCore.Mvc;
using Xunit;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Animal;
using ZooManagerWeb.Domain.Value_Object.Enclosure;
using ZooManagerWeb.Infrastructure.Repositories;
using ZooManagerWeb.Presentation.Controllers;

public class StatisticsControllerTests
{
    private readonly InMemoryAnimalRepository _animalRepository;
    private readonly InMemoryEnclosureRepository _enclosureRepository;
    private readonly InMemoryFeedingScheduleRepository _feedingScheduleRepository;
    private readonly StatisticsController _controller;

    public StatisticsControllerTests()
    {
        _animalRepository = new InMemoryAnimalRepository();
        _enclosureRepository = new InMemoryEnclosureRepository();
        _feedingScheduleRepository = new InMemoryFeedingScheduleRepository();
        _controller = new StatisticsController(_animalRepository, _enclosureRepository, _feedingScheduleRepository);
    }

    [Fact]
    public async Task GetBasicStatistics_ShouldReturnCorrectStatistics()
    {
        // Arrange
        var animal1 = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var animal2 = new Animal("Тигр", "Шершень", DateTime.Now, Gender.Female, "Мясо", HealthStatus.Healthy);
        var animal3 = new Animal("Слон", "Чилли", DateTime.Now, Gender.Male, "Овощи", HealthStatus.Sick);

        await _animalRepository.AddAsync(animal1);
        await _animalRepository.AddAsync(animal2);
        await _animalRepository.AddAsync(animal3);
        
        var enclosure1 = new Enclosure(EnclosureType.Predator, 100, 3);
        var enclosure2 = new Enclosure(EnclosureType.Herbivore, 150, 5);
        var enclosure3 = new Enclosure(EnclosureType.Aquarium, 200, 10);

        await _enclosureRepository.AddAsync(enclosure1);
        await _enclosureRepository.AddAsync(enclosure2);
        await _enclosureRepository.AddAsync(enclosure3);
        
        enclosure1.AddAnimal(animal1.Id);
        animal1.UpdateEnclosure(enclosure1.Id);
        enclosure1.AddAnimal(animal2.Id);
        animal2.UpdateEnclosure(enclosure1.Id);

        await _animalRepository.UpdateAsync(animal1);
        await _animalRepository.UpdateAsync(animal2);
        await _enclosureRepository.UpdateAsync(enclosure1);

        // Act
        var actionResult = await _controller.GetBasicStatistics();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var statistics = Assert.IsType<ZooStatistics>(okResult.Value);

        Assert.Equal(3, statistics.TotalAnimals);
        Assert.Equal(3, statistics.TotalEnclosures);
        Assert.Equal(2, statistics.FreeEnclosures);
        Assert.Equal(1, statistics.OccupiedEnclosures);
        Assert.Equal(1.0, statistics.AverageEnclosureOccupancy);
    }

    [Fact]
    public async Task GetDetailedStatistics_ShouldReturnCorrectStatistics()
    {
        // Arrange
        var animal1 = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var animal2 = new Animal("Лев", "Найл", DateTime.Now, Gender.Female, "Мясо", HealthStatus.Healthy);
        var animal3 = new Animal("Слон", "Чилли", DateTime.Now, Gender.Male, "Овощи", HealthStatus.Sick);

        await _animalRepository.AddAsync(animal1);
        await _animalRepository.AddAsync(animal2);
        await _animalRepository.AddAsync(animal3);
        
        var enclosure1 = new Enclosure(EnclosureType.Predator, 100, 3);
        var enclosure2 = new Enclosure(EnclosureType.Herbivore, 150, 5);

        await _enclosureRepository.AddAsync(enclosure1);
        await _enclosureRepository.AddAsync(enclosure2);
        
        enclosure1.AddAnimal(animal1.Id);
        animal1.UpdateEnclosure(enclosure1.Id);
        enclosure1.AddAnimal(animal2.Id);
        animal2.UpdateEnclosure(enclosure1.Id);
        enclosure2.AddAnimal(animal3.Id);
        animal3.UpdateEnclosure(enclosure2.Id);

        await _animalRepository.UpdateAsync(animal1);
        await _animalRepository.UpdateAsync(animal2);
        await _animalRepository.UpdateAsync(animal3);
        await _enclosureRepository.UpdateAsync(enclosure1);
        await _enclosureRepository.UpdateAsync(enclosure2);
        
        var schedule1 = new FeedingSchedule(animal1.Id, "Мясо", DateTime.UtcNow);
        var schedule2 = new FeedingSchedule(animal2.Id, "Мясо", DateTime.UtcNow.AddHours(1));
        schedule2.isCompleted();

        await _feedingScheduleRepository.AddAsync(schedule1);
        await _feedingScheduleRepository.AddAsync(schedule2);

        // Act
        var actionResult = await _controller.GetDetailedStatistics();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var statistics = Assert.IsType<DetailedZooStatistics>(okResult.Value);
        
        Assert.Equal(3, statistics.BasicStatistics.TotalAnimals);
        Assert.Equal(2, statistics.BasicStatistics.TotalEnclosures);
        Assert.Equal(0, statistics.BasicStatistics.FreeEnclosures);
        Assert.Equal(2, statistics.BasicStatistics.OccupiedEnclosures);
        
        Assert.Equal(2, statistics.SpeciesDistribution.Count);
        Assert.Equal(2, statistics.SpeciesDistribution["Лев"]);
        Assert.Equal(1, statistics.SpeciesDistribution["Слон"]);
        
        Assert.Equal(2, statistics.EnclosureTypeDistribution.Count);
        Assert.Equal(1, statistics.EnclosureTypeDistribution["Predator"]);
        Assert.Equal(1, statistics.EnclosureTypeDistribution["Herbivore"]);
        
        Assert.Equal(2, statistics.FeedingStatistics.TotalSchedules);
        Assert.Equal(1, statistics.FeedingStatistics.CompletedFeedings);
        Assert.Equal(1, statistics.FeedingStatistics.PendingFeedings);
        Assert.Equal(50.0, statistics.FeedingStatistics.CompletionRate);
    }
}