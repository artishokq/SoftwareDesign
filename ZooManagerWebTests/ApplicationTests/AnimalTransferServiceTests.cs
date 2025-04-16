namespace ZooManagerWebTests.ApplicationTests;

using Xunit;
using ZooManagerWeb.Application.Services;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Animal;
using ZooManagerWeb.Domain.Value_Object.Enclosure;
using ZooManagerWeb.Infrastructure.Repositories;

public class AnimalTransferServiceTests
{
    private readonly InMemoryAnimalRepository _animalRepository;
    private readonly InMemoryEnclosureRepository _enclosureRepository;
    private readonly AnimalTransferService _service;

    public AnimalTransferServiceTests()
    {
        _animalRepository = new InMemoryAnimalRepository();
        _enclosureRepository = new InMemoryEnclosureRepository();
        _service = new AnimalTransferService(_animalRepository, _enclosureRepository);
    }

    [Fact]
    public async Task TransferAnimalAsync_ShouldTransferAnimalToNewEnclosure()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);

        var sourceEnclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _enclosureRepository.AddAsync(sourceEnclosure);

        var targetEnclosure = new Enclosure(EnclosureType.Predator, 150, 5);
        await _enclosureRepository.AddAsync(targetEnclosure);
        
        animal.UpdateEnclosure(sourceEnclosure.Id);
        sourceEnclosure.AddAnimal(animal.Id);
        await _animalRepository.UpdateAsync(animal);
        await _enclosureRepository.UpdateAsync(sourceEnclosure);

        bool eventRaised = false;
        _service.AnimalMoved += (e) => eventRaised = true;

        // Act
        bool result = await _service.TransferAnimalAsync(animal.Id, targetEnclosure.Id);
        
        var updatedAnimal = await _animalRepository.GetByIdAsync(animal.Id);
        var updatedSourceEnclosure = await _enclosureRepository.GetByIdAsync(sourceEnclosure.Id);
        var updatedTargetEnclosure = await _enclosureRepository.GetByIdAsync(targetEnclosure.Id);

        // Assert
        Assert.True(result);
        Assert.Equal(targetEnclosure.Id, updatedAnimal.CurrentEnclosureId);
        Assert.Equal(0, updatedSourceEnclosure.CurrentAnimalNumber);
        Assert.Equal(1, updatedTargetEnclosure.CurrentAnimalNumber);
        Assert.Contains(animal.Id, updatedTargetEnclosure.AnimalIds);
        Assert.True(eventRaised);
    }

    [Fact]
    public async Task TransferAnimalAsync_ShouldReturnFalse_WhenAnimalNotFound()
    {
        // Arrange
        var nonExistentAnimalId = Guid.NewGuid();
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _enclosureRepository.AddAsync(enclosure);

        // Act
        bool result = await _service.TransferAnimalAsync(nonExistentAnimalId, enclosure.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TransferAnimalAsync_ShouldReturnFalse_WhenEnclosureNotFound()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);

        var nonExistentEnclosureId = Guid.NewGuid();

        // Act
        bool result = await _service.TransferAnimalAsync(animal.Id, nonExistentEnclosureId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TransferAnimalAsync_ShouldReturnFalse_WhenEnclosureIsAtCapacity()
    {
        // Arrange
        var animal1 = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var animal2 = new Animal("Тигр", "Шершень", DateTime.Now, Gender.Female, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal1);
        await _animalRepository.AddAsync(animal2);

        var enclosure = new Enclosure(EnclosureType.Predator, 100, 1); // Capacity = 1
        await _enclosureRepository.AddAsync(enclosure);

        // Заполняем вольер до предела
        enclosure.AddAnimal(animal1.Id);
        animal1.UpdateEnclosure(enclosure.Id);
        await _animalRepository.UpdateAsync(animal1);
        await _enclosureRepository.UpdateAsync(enclosure);

        // Act
        bool result = await _service.TransferAnimalAsync(animal2.Id, enclosure.Id);

        // Assert
        Assert.False(result);
        var updatedEnclosure = await _enclosureRepository.GetByIdAsync(enclosure.Id);
        Assert.Equal(1, updatedEnclosure.CurrentAnimalNumber);
        Assert.DoesNotContain(animal2.Id, updatedEnclosure.AnimalIds);
    }
}