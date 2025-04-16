namespace ZooManagerWebTests.DomainTests;

using Xunit;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Enclosure;


public class EnclosureTests
{
    [Fact]
    public void Constructor_ShouldCreateEnclosure_WithCorrectProperties()
    {
        // Arrange
        EnclosureType type = EnclosureType.Predator;
        int size = 100;
        int capacity = 5;

        // Act
        var enclosure = new Enclosure(type, size, capacity);

        // Assert
        Assert.Equal(type, enclosure.Type);
        Assert.Equal(size, enclosure.Size);
        Assert.Equal(capacity, enclosure.Capacity);
        Assert.NotEqual(Guid.Empty, enclosure.Id);
        Assert.Equal(0, enclosure.CurrentAnimalNumber);
        Assert.Empty(enclosure.AnimalIds);
    }

    [Fact]
    public void AddAnimal_ShouldAddAnimalId_WhenEnclosureHasCapacity()
    {
        // Arrange
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 2);
        var animalId = Guid.NewGuid();

        // Act
        var result = enclosure.AddAnimal(animalId);

        // Assert
        Assert.True(result);
        Assert.Equal(1, enclosure.CurrentAnimalNumber);
        Assert.Contains(animalId, enclosure.AnimalIds);
    }

    [Fact]
    public void AddAnimal_ShouldReturnFalse_WhenEnclosureIsAtCapacity()
    {
        // Arrange
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 1);
        var animalId1 = Guid.NewGuid();
        var animalId2 = Guid.NewGuid();
        enclosure.AddAnimal(animalId1);

        // Act
        var result = enclosure.AddAnimal(animalId2);

        // Assert
        Assert.False(result);
        Assert.Equal(1, enclosure.CurrentAnimalNumber);
        Assert.DoesNotContain(animalId2, enclosure.AnimalIds);
    }

    [Fact]
    public void DeleteAnimal_ShouldRemoveAnimalId_WhenAnimalExists()
    {
        // Arrange
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 2);
        var animalId = Guid.NewGuid();
        enclosure.AddAnimal(animalId);

        // Act
        var result = enclosure.DeleteAnimal(animalId);

        // Assert
        Assert.True(result);
        Assert.Equal(0, enclosure.CurrentAnimalNumber);
        Assert.DoesNotContain(animalId, enclosure.AnimalIds);
    }

    [Fact]
    public void DeleteAnimal_ShouldReturnFalse_WhenAnimalDoesNotExist()
    {
        // Arrange
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 2);
        var animalId = Guid.NewGuid();

        // Act
        var result = enclosure.DeleteAnimal(animalId);

        // Assert
        Assert.False(result);
        Assert.Equal(0, enclosure.CurrentAnimalNumber);
    }
}