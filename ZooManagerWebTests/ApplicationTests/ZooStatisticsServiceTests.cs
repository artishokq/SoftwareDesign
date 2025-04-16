namespace ZooManagerWebTests.ApplicationTests;

using Xunit;
using ZooManagerWeb.Application.Services;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Animal;
using ZooManagerWeb.Domain.Value_Object.Enclosure;
using ZooManagerWeb.Infrastructure.Repositories;

public class ZooStatisticsServiceTests
{
    private readonly InMemoryAnimalRepository _animalRepository;
    private readonly InMemoryEnclosureRepository _enclosureRepository;
    private readonly InMemoryFeedingScheduleRepository _feedingScheduleRepository;
    private readonly ZooStatisticsService _service;

    public ZooStatisticsServiceTests()
    {
        _animalRepository = new InMemoryAnimalRepository();
        _enclosureRepository = new InMemoryEnclosureRepository();
        _feedingScheduleRepository = new InMemoryFeedingScheduleRepository();
        _service = new ZooStatisticsService(_animalRepository, _enclosureRepository, _feedingScheduleRepository);
    }

    [Fact]
    public async Task GetTotalAnimalCountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        await _animalRepository.AddAsync(new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Тигр", "Шершень", DateTime.Now, Gender.Female, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Слон", "Чилли", DateTime.Now, Gender.Male, "Овощи",
            HealthStatus.Sick));

        // Act
        int totalCount = await _service.GetTotalAnimalCountAsync();

        // Assert
        Assert.Equal(3, totalCount);
    }

    [Fact]
    public async Task GetAnimalCountBySpeciesAsync_ShouldReturnCorrectCounts()
    {
        // Arrange
        await _animalRepository.AddAsync(new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Лев", "Найл", DateTime.Now, Gender.Female, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Тигр", "Шершень", DateTime.Now, Gender.Male, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Слон", "Чилли", DateTime.Now, Gender.Male, "Овощи",
            HealthStatus.Sick));

        // Act
        var speciesCounts = await _service.GetAnimalCountBySpeciesAsync();

        // Assert
        Assert.Equal(3, speciesCounts.Count);
        Assert.Equal(2, speciesCounts["Лев"]);
        Assert.Equal(1, speciesCounts["Тигр"]);
        Assert.Equal(1, speciesCounts["Слон"]);
    }

    [Fact]
    public async Task GetGenderDistributionAsync_ShouldReturnCorrectDistribution()
    {
        // Arrange
        await _animalRepository.AddAsync(new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Лев", "Найл", DateTime.Now, Gender.Female, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Тигр", "Шершень", DateTime.Now, Gender.Male, "Мясо",
            HealthStatus.Healthy));

        // Act
        var genderDistribution = await _service.GetGenderDistributionAsync();

        // Assert
        Assert.Equal(2, genderDistribution.Count);
        Assert.Equal(2, genderDistribution[Gender.Male]);
        Assert.Equal(1, genderDistribution[Gender.Female]);
    }

    [Fact]
    public async Task GetHealthStatusDistributionAsync_ShouldReturnCorrectDistribution()
    {
        // Arrange
        await _animalRepository.AddAsync(new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Лев", "Найл", DateTime.Now, Gender.Female, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Тигр", "Шершень", DateTime.Now, Gender.Male, "Мясо",
            HealthStatus.Sick));

        // Act
        var healthDistribution = await _service.GetHealthStatusDistributionAsync();

        // Assert
        Assert.Equal(2, healthDistribution.Count);
        Assert.Equal(2, healthDistribution[HealthStatus.Healthy]);
        Assert.Equal(1, healthDistribution[HealthStatus.Sick]);
    }

    [Fact]
    public async Task GetAgeGroupDistributionAsync_ShouldReturnCorrectDistribution()
    {
        // Arrange
        var now = DateTime.UtcNow;
        await _animalRepository.AddAsync(new Animal("Лев", "Симба", now.AddYears(-1), Gender.Male, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Лев", "Найл", now.AddYears(-5), Gender.Female, "Мясо",
            HealthStatus.Healthy));
        await _animalRepository.AddAsync(new Animal("Тигр", "Шершень", now.AddYears(-12), Gender.Male, "Мясо",
            HealthStatus.Sick));

        // Act
        var ageDistribution = await _service.GetAgeGroupDistributionAsync();

        // Assert
        Assert.Equal(3, ageDistribution.Count);
        Assert.Equal(1, ageDistribution["Молодой (< 2 лет)"]);
        Assert.Equal(1, ageDistribution["Взрослый (2-10 лет)"]);
        Assert.Equal(1, ageDistribution["Старый (> 10 лет)"]);
    }

    [Fact]
    public async Task GetEnclosureStatisticsAsync_ShouldReturnCorrectStatistics()
    {
        // Arrange
        var enclosure1 = new Enclosure(EnclosureType.Predator, 100, 3);
        var enclosure2 = new Enclosure(EnclosureType.Herbivore, 150, 5);
        var enclosure3 = new Enclosure(EnclosureType.Predator, 120, 2);

        await _enclosureRepository.AddAsync(enclosure1);
        await _enclosureRepository.AddAsync(enclosure2);
        await _enclosureRepository.AddAsync(enclosure3);

        var animal1 = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var animal2 = new Animal("Лев", "Найл", DateTime.Now, Gender.Female, "Мясо", HealthStatus.Healthy);
        var animal3 = new Animal("Слон", "Чилли", DateTime.Now, Gender.Male, "Овощи", HealthStatus.Healthy);

        await _animalRepository.AddAsync(animal1);
        await _animalRepository.AddAsync(animal2);
        await _animalRepository.AddAsync(animal3);

        // Добавляем животных в вольеры
        enclosure1.AddAnimal(animal1.Id);
        enclosure1.AddAnimal(animal2.Id);
        enclosure2.AddAnimal(animal3.Id);

        await _enclosureRepository.UpdateAsync(enclosure1);
        await _enclosureRepository.UpdateAsync(enclosure2);

        // Act
        var enclosureStats = await _service.GetEnclosureStatisticsAsync();

        // Assert
        Assert.Equal(3, enclosureStats["TotalEnclosures"]);
        Assert.Equal(10, enclosureStats["TotalCapacity"]);
        Assert.Equal(3, enclosureStats["OccupiedCapacity"]);

        var enclosuresByType = (Dictionary<string, int>)enclosureStats["EnclosuresByType"];
        Assert.Equal(2, enclosuresByType["Predator"]);
        Assert.Equal(1, enclosuresByType["Herbivore"]);

        var occupancyRates = (Dictionary<string, double>)enclosureStats["OccupancyRates"];
        Assert.Equal(66.7, occupancyRates[enclosure1.Id.ToString()], 1);
        Assert.Equal(20.0, occupancyRates[enclosure2.Id.ToString()], 1);
        Assert.Equal(0.0, occupancyRates[enclosure3.Id.ToString()], 1);
    }
}