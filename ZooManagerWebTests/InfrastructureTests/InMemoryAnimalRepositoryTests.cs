namespace ZooManagerWebTests.InfrastructureTests;

using Xunit;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Animal;
using ZooManagerWeb.Infrastructure.Repositories;

public class InMemoryAnimalRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenRepositoryIsEmpty()
    {
        // Arrange
        var repository = new InMemoryAnimalRepository();

        // Act
        var animals = await repository.GetAllAsync();

        // Assert
        Assert.Empty(animals);
    }

    [Fact]
    public async Task AddAsync_ShouldAddAnimal_ToRepository()
    {
        // Arrange
        var repository = new InMemoryAnimalRepository();
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);

        // Act
        await repository.AddAsync(animal);
        var animals = await repository.GetAllAsync();

        // Assert
        Assert.Single(animals);
        Assert.Same(animal, animals.First());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAnimal_WhenExists()
    {
        // Arrange
        var repository = new InMemoryAnimalRepository();
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await repository.AddAsync(animal);

        // Act
        var result = await repository.GetByIdAsync(animal.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Same(animal, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        var repository = new InMemoryAnimalRepository();

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEnclosureIdAsync_ShouldReturnAnimals_InEnclosure()
    {
        // Arrange
        var repository = new InMemoryAnimalRepository();
        var enclosureId = Guid.NewGuid();

        var animal1 = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        animal1.UpdateEnclosure(enclosureId);

        var animal2 = new Animal("Слон", "Чилли", DateTime.Now, Gender.Male, "Овощи", HealthStatus.Healthy);
        animal2.UpdateEnclosure(enclosureId);

        var animal3 = new Animal("Тигр", "Шершень", DateTime.Now, Gender.Female, "Мясо", HealthStatus.Healthy);
        animal3.UpdateEnclosure(Guid.NewGuid());

        await repository.AddAsync(animal1);
        await repository.AddAsync(animal2);
        await repository.AddAsync(animal3);

        // Act
        var result = await repository.GetByEnclosureIdAsync(enclosureId);
        var animals = result.ToList();

        // Assert
        Assert.Equal(2, animals.Count);
        Assert.Contains(animal1, animals);
        Assert.Contains(animal2, animals);
        Assert.DoesNotContain(animal3, animals);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAnimal_WhenExists()
    {
        // Arrange
        var repository = new InMemoryAnimalRepository();
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Sick);
        await repository.AddAsync(animal);

        // Act
        animal.Heal();
        await repository.UpdateAsync(animal);
        var updatedAnimal = await repository.GetByIdAsync(animal.Id);

        // Assert
        Assert.Equal(HealthStatus.Healthy, updatedAnimal.HealthStatus);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAnimal_WhenExists()
    {
        // Arrange
        var repository = new InMemoryAnimalRepository();
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await repository.AddAsync(animal);

        // Act
        await repository.DeleteAsync(animal.Id);
        var result = await repository.GetByIdAsync(animal.Id);
        var allAnimals = await repository.GetAllAsync();

        // Assert
        Assert.Null(result);
        Assert.Empty(allAnimals);
    }
}