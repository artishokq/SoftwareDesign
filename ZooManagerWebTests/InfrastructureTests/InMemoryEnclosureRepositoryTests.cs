namespace ZooManagerWebTests.InfrastructureTests;

using Xunit;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Enclosure;
using ZooManagerWeb.Infrastructure.Repositories;

public class InMemoryEnclosureRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenRepositoryIsEmpty()
    {
        // Arrange
        var repository = new InMemoryEnclosureRepository();

        // Act
        var enclosures = await repository.GetAllAsync();

        // Assert
        Assert.Empty(enclosures);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEnclosure_ToRepository()
    {
        // Arrange
        var repository = new InMemoryEnclosureRepository();
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);

        // Act
        await repository.AddAsync(enclosure);
        var enclosures = await repository.GetAllAsync();

        // Assert
        Assert.Single(enclosures);
        Assert.Same(enclosure, enclosures.First());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEnclosure_WhenExists()
    {
        // Arrange
        var repository = new InMemoryEnclosureRepository();
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await repository.AddAsync(enclosure);

        // Act
        var result = await repository.GetByIdAsync(enclosure.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Same(enclosure, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        var repository = new InMemoryEnclosureRepository();

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEnclosure_WhenExists()
    {
        // Arrange
        var repository = new InMemoryEnclosureRepository();
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await repository.AddAsync(enclosure);

        // Act - добавим животное в вольер
        var animalId = Guid.NewGuid();
        enclosure.AddAnimal(animalId);
        await repository.UpdateAsync(enclosure);

        // Проверяем, что вольер обновился
        var updatedEnclosure = await repository.GetByIdAsync(enclosure.Id);

        // Assert
        Assert.Equal(1, updatedEnclosure.CurrentAnimalNumber);
        Assert.Contains(animalId, updatedEnclosure.AnimalIds);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEnclosure_WhenExists()
    {
        // Arrange
        var repository = new InMemoryEnclosureRepository();
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await repository.AddAsync(enclosure);

        // Act
        await repository.DeleteAsync(enclosure.Id);
        var result = await repository.GetByIdAsync(enclosure.Id);
        var allEnclosures = await repository.GetAllAsync();

        // Assert
        Assert.Null(result);
        Assert.Empty(allEnclosures);
    }
}