namespace ZooManagerWebTests.PresentationTests;

using Microsoft.AspNetCore.Mvc;
using Xunit;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Value_Object.Animal;
using ZooManagerWeb.Domain.Value_Object.Enclosure;
using ZooManagerWeb.Infrastructure.Repositories;
using ZooManagerWeb.Presentation.Controllers;

public class EnclosuresControllerTests
{
    private readonly InMemoryAnimalRepository _animalRepository;
    private readonly InMemoryEnclosureRepository _enclosureRepository;
    private readonly EnclosuresController _controller;

    public EnclosuresControllerTests()
    {
        _animalRepository = new InMemoryAnimalRepository();
        _enclosureRepository = new InMemoryEnclosureRepository();
        _controller = new EnclosuresController(_enclosureRepository, _animalRepository);
    }

    [Fact]
    public async Task GetEnclosures_ShouldReturnAllEnclosures()
    {
        // Arrange
        var enclosure1 = new Enclosure(EnclosureType.Predator, 100, 5);
        var enclosure2 = new Enclosure(EnclosureType.Herbivore, 150, 10);
        await _enclosureRepository.AddAsync(enclosure1);
        await _enclosureRepository.AddAsync(enclosure2);

        // Act
        var actionResult = await _controller.GetEnclosures();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var enclosures = Assert.IsAssignableFrom<IEnumerable<EnclosureResponse>>(okResult.Value);
        Assert.Equal(2, enclosures.Count());
    }

    [Fact]
    public async Task GetEnclosure_ShouldReturnEnclosure_WhenExists()
    {
        // Arrange
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _enclosureRepository.AddAsync(enclosure);

        // Act
        var actionResult = await _controller.GetEnclosure(enclosure.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var response = Assert.IsType<EnclosureResponse>(okResult.Value);
        Assert.Equal(enclosure.Id, response.Id);
        Assert.Equal(enclosure.Type, response.Type);
        Assert.Equal(enclosure.Size, response.Size);
        Assert.Equal(enclosure.Capacity, response.Capacity);
    }

    [Fact]
    public async Task GetEnclosure_ShouldReturnNotFound_WhenDoesNotExist()
    {
        // Act
        var actionResult = await _controller.GetEnclosure(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task CreateEnclosure_ShouldCreateAndReturnEnclosure()
    {
        // Arrange
        var request = new EnclosureCreateRequest
        {
            Type = EnclosureType.Predator,
            Size = 100,
            Capacity = 5
        };

        // Act
        var actionResult = await _controller.CreateEnclosure(request);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnedEnclosure = Assert.IsType<Enclosure>(createdAtActionResult.Value);
        Assert.Equal(request.Type, returnedEnclosure.Type);
        Assert.Equal(request.Size, returnedEnclosure.Size);
        Assert.Equal(request.Capacity, returnedEnclosure.Capacity);
        var savedEnclosure = await _enclosureRepository.GetByIdAsync(returnedEnclosure.Id);
        Assert.NotNull(savedEnclosure);
    }

    [Fact]
    public async Task AddAnimalToEnclosure_ShouldAddAnimal_WhenValidRequest()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        await _animalRepository.AddAsync(animal);
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _enclosureRepository.AddAsync(enclosure);

        // Act
        var result = await _controller.AddAnimalToEnclosure(enclosure.Id, animal.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedAnimal = await _animalRepository.GetByIdAsync(animal.Id);
        var updatedEnclosure = await _enclosureRepository.GetByIdAsync(enclosure.Id);
        Assert.Equal(enclosure.Id, updatedAnimal.CurrentEnclosureId);
        Assert.Equal(1, updatedEnclosure.CurrentAnimalNumber);
        Assert.Contains(animal.Id, updatedEnclosure.AnimalIds);
    }

    [Fact]
    public async Task AddAnimalToEnclosure_ShouldReturnBadRequest_WhenAnimalAlreadyInEnclosure()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _animalRepository.AddAsync(animal);
        await _enclosureRepository.AddAsync(enclosure);
        var otherEnclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _enclosureRepository.AddAsync(otherEnclosure);
        animal.UpdateEnclosure(otherEnclosure.Id);
        await _animalRepository.UpdateAsync(animal);

        // Act
        var result = await _controller.AddAnimalToEnclosure(enclosure.Id, animal.Id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RemoveAnimalFromEnclosure_ShouldRemoveAnimal_WhenValidRequest()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _animalRepository.AddAsync(animal);
        await _enclosureRepository.AddAsync(enclosure);
        enclosure.AddAnimal(animal.Id);
        animal.UpdateEnclosure(enclosure.Id);
        await _animalRepository.UpdateAsync(animal);
        await _enclosureRepository.UpdateAsync(enclosure);

        // Act
        var result = await _controller.RemoveAnimalFromEnclosure(enclosure.Id, animal.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedAnimal = await _animalRepository.GetByIdAsync(animal.Id);
        var updatedEnclosure = await _enclosureRepository.GetByIdAsync(enclosure.Id);
        Assert.Null(updatedAnimal.CurrentEnclosureId);
        Assert.Equal(0, updatedEnclosure.CurrentAnimalNumber);
        Assert.DoesNotContain(animal.Id, updatedEnclosure.AnimalIds);
    }

    [Fact]
    public async Task DeleteEnclosure_ShouldDeleteEnclosure_WhenEmpty()
    {
        // Arrange
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _enclosureRepository.AddAsync(enclosure);

        // Act
        var result = await _controller.DeleteEnclosure(enclosure.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var deletedEnclosure = await _enclosureRepository.GetByIdAsync(enclosure.Id);
        Assert.Null(deletedEnclosure);
    }

    [Fact]
    public async Task DeleteEnclosure_ShouldReturnBadRequest_WhenContainsAnimals()
    {
        // Arrange
        var animal = new Animal("Лев", "Симба", DateTime.Now, Gender.Male, "Мясо", HealthStatus.Healthy);
        var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
        await _animalRepository.AddAsync(animal);
        await _enclosureRepository.AddAsync(enclosure);
        enclosure.AddAnimal(animal.Id);
        animal.UpdateEnclosure(enclosure.Id);
        await _animalRepository.UpdateAsync(animal);
        await _enclosureRepository.UpdateAsync(enclosure);

        // Act
        var result = await _controller.DeleteEnclosure(enclosure.Id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var existingEnclosure = await _enclosureRepository.GetByIdAsync(enclosure.Id);
        Assert.NotNull(existingEnclosure);
    }
}