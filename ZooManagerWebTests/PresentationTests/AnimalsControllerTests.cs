namespace ZooManagerWebTests.PresentationTests;

using Microsoft.AspNetCore.Mvc;
using Xunit;
using ZooManagerWeb.Application.Services;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Animal;
using ZooManagerWeb.Domain.Value_Object.Enclosure;
using ZooManagerWeb.Infrastructure.Repositories;
using ZooManagerWeb.Presentation.Controllers;

public class AnimalsControllerTests
{
    private readonly InMemoryAnimalRepository _animalRepository;
    private readonly InMemoryEnclosureRepository _enclosureRepository;
    private readonly AnimalTransferService _transferService;
    private readonly AnimalsController _controller;

    public AnimalsControllerTests()
    {
        _animalRepository = new InMemoryAnimalRepository();
        _enclosureRepository = new InMemoryEnclosureRepository();
        _transferService = new AnimalTransferService(_animalRepository, _enclosureRepository);
        _controller = new AnimalsController(_animalRepository, _transferService);
    }

    [Fact]
    public async Task GetAnimals_ShouldReturnAllAnimals()
    {
        // Arrange
        var animal1 = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var animal2 = new Animal("Тигр", "Шершень", DateTime.Now, Gender.Female, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal1);
        await _animalRepository.AddAsync(animal2);

        // Act
        var actionResult = await _controller.GetAnimals();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var animals = Assert.IsAssignableFrom<IEnumerable<Animal>>(okResult.Value);
        Assert.Equal(2, animals.Count());
        Assert.Contains(animal1, animals);
        Assert.Contains(animal2, animals);
    }

    [Fact]
    public async Task GetAnimal_ShouldReturnAnimal_WhenExists()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);

        // Act
        var actionResult = await _controller.GetAnimal(animal.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedAnimal = Assert.IsType<Animal>(okResult.Value);
        Assert.Equal(animal.Id, returnedAnimal.Id);
        Assert.Equal(animal.Name, returnedAnimal.Name);
    }

    [Fact]
    public async Task GetAnimal_ShouldReturnNotFound_WhenDoesNotExist()
    {
        // Act
        var actionResult = await _controller.GetAnimal(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task CreateAnimal_ShouldCreateAndReturnAnimal()
    {
        // Arrange
        var request = new AnimalCreateRequest
        {
            Species = "Лев",
            Name = "Симба",
            Birthday = DateTime.Now,
            Gender = Gender.Male,
            FavFood = "Мясо",
            HealthStatus = HealthStatus.Healthy
        };

        // Act
        var actionResult = await _controller.CreateAnimal(request);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnedAnimal = Assert.IsType<Animal>(createdAtActionResult.Value);

        Assert.Equal(request.Species, returnedAnimal.Species);
        Assert.Equal(request.Name, returnedAnimal.Name);
        Assert.Equal(request.Birthday, returnedAnimal.Birthday);
        Assert.Equal(request.Gender, returnedAnimal.Gender);
        Assert.Equal(request.FavFood, returnedAnimal.FavFood);
        Assert.Equal(request.HealthStatus, returnedAnimal.HealthStatus);
        var savedAnimal = await _animalRepository.GetByIdAsync(returnedAnimal.Id);
        Assert.NotNull(savedAnimal);
    }

    [Fact]
    public async Task UpdateAnimalHealth_ShouldHealAnimal_WhenExists()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Sick);
        await _animalRepository.AddAsync(animal);

        // Act
        var result = await _controller.UpdateAnimalHealth(animal.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedAnimal = await _animalRepository.GetByIdAsync(animal.Id);
        Assert.Equal(HealthStatus.Healthy, updatedAnimal.HealthStatus);
    }

    [Fact]
    public async Task UpdateAnimalHealth_ShouldReturnNotFound_WhenDoesNotExist()
    {
        // Act
        var result = await _controller.UpdateAnimalHealth(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task TransferAnimal_ShouldTransferAnimal_WhenValidRequest()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _enclosureRepository.AddAsync(enclosure);

        // Act
        var result = await _controller.TransferAnimal(animal.Id, enclosure.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedAnimal = await _animalRepository.GetByIdAsync(animal.Id);
        var updatedEnclosure = await _enclosureRepository.GetByIdAsync(enclosure.Id);
        Assert.Equal(enclosure.Id, updatedAnimal.CurrentEnclosureId);
        Assert.Equal(1, updatedEnclosure.CurrentAnimalNumber);
        Assert.Contains(animal.Id, updatedEnclosure.AnimalIds);
    }

    [Fact]
    public async Task TransferAnimal_ShouldReturnBadRequest_WhenTransferFails()
    {
        // Arrange
        var animal = new Animal("Мясо", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 0);
        await _enclosureRepository.AddAsync(enclosure);

        // Act
        var result = await _controller.TransferAnimal(animal.Id, enclosure.Id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteAnimal_ShouldDeleteAnimal_WhenExists()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);

        // Act
        var result = await _controller.DeleteAnimal(animal.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var deletedAnimal = await _animalRepository.GetByIdAsync(animal.Id);
        Assert.Null(deletedAnimal);
    }

    [Fact]
    public async Task DeleteAnimal_ShouldReturnNotFound_WhenDoesNotExist()
    {
        // Act
        var result = await _controller.DeleteAnimal(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}