namespace ZooManagerWebTests.DomainTests;

using Xunit;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Animal;

public class AnimalTests
{
    [Fact]
    public void Constructor_ShouldCreateAnimal_WithCorrectProperties()
    {
        // Arrange
        string species = "Лев";
        string name = "Симба";
        DateTime birthday = new DateTime(2020, 1, 1);
        Gender gender = Gender.Male;
        string favFood = "Мясо";
        HealthStatus health = HealthStatus.Healthy;

        // Act
        var animal = new Animal(species, name, birthday, gender, favFood, health);

        // Assert
        Assert.Equal(species, animal.Species);
        Assert.Equal(name, animal.Name);
        Assert.Equal(birthday, animal.Birthday);
        Assert.Equal(gender, animal.Gender);
        Assert.Equal(favFood, animal.FavFood);
        Assert.Equal(health, animal.HealthStatus);
        Assert.NotEqual(Guid.Empty, animal.Id);
        Assert.Null(animal.CurrentEnclosureId);
    }

    [Fact]
    public void Heal_ShouldChangeHealthStatusToHealthy_WhenAnimalIsSick()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Sick);

        // Act
        animal.Heal();

        // Assert
        Assert.Equal(HealthStatus.Healthy, animal.HealthStatus);
    }

    [Fact]
    public void Heal_ShouldNotChangeHealthStatus_WhenAnimalIsAlreadyHealthy()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);

        // Act
        animal.Heal();

        // Assert
        Assert.Equal(HealthStatus.Healthy, animal.HealthStatus);
    }

    [Fact]
    public void UpdateEnclosure_ShouldSetCurrentEnclosureId()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var enclosureId = Guid.NewGuid();

        // Act
        animal.UpdateEnclosure(enclosureId);

        // Assert
        Assert.Equal(enclosureId, animal.CurrentEnclosureId);
    }

    [Fact]
    public void UpdateEnclosure_ShouldSetCurrentEnclosureIdToNull_WhenCalledWithNull()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var enclosureId = Guid.NewGuid();
        animal.UpdateEnclosure(enclosureId);

        // Act
        animal.UpdateEnclosure(null);

        // Assert
        Assert.Null(animal.CurrentEnclosureId);
    }
}