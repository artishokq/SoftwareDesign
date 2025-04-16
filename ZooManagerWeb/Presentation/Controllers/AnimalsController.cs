namespace ZooManagerWeb.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Infrastructure.Interfaces;
using ZooManagerWeb.Application.Services;
using ZooManagerWeb.Domain.Value_Object.Animal;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    private readonly AnimalTransferService _transferService;

    public AnimalsController(
        IAnimalRepository animalRepository,
        AnimalTransferService transferService)
    {
        _animalRepository = animalRepository;
        _transferService = transferService;
    }

    /// <summary>
    /// Получает список всех животных в зоопарке
    /// </summary>
    /// <returns>Коллекция всех животных</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
    {
        var animals = await _animalRepository.GetAllAsync();
        return Ok(animals);
    }

    /// <summary>
    /// Получает информацию о конкретном животном по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор животного</param>
    /// <returns>Данные о животном или 404, если животное не найдено</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetAnimal(Guid id)
    {
        var animal = await _animalRepository.GetByIdAsync(id);

        if (animal == null)
        {
            return NotFound();
        }

        return Ok(animal);
    }

    /// <summary>
    /// Получает список всех животных, находящихся в указанном вольере
    /// </summary>
    /// <param name="enclosureId">Идентификатор вольера</param>
    /// <returns>Коллекция животных в указанном вольере</returns>
    [HttpGet("enclosure/{enclosureId}")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimalsByEnclosure(Guid enclosureId)
    {
        var animals = await _animalRepository.GetByEnclosureIdAsync(enclosureId);
        return Ok(animals);
    }

    /// <summary>
    /// Создает новое животное в системе
    /// </summary>
    /// <param name="request">Данные для создания животного</param>
    /// <returns>Созданное животное с присвоенным идентификатором</returns>
    [HttpPost]
    public async Task<ActionResult<Animal>> CreateAnimal(AnimalCreateRequest request)
    {
        var animal = new Animal(
            request.Species,
            request.Name,
            request.Birthday,
            request.Gender,
            request.FavFood,
            request.HealthStatus
        );

        await _animalRepository.AddAsync(animal);

        return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
    }

    /// <summary>
    /// Изменяет статус здоровья животного на "Здоровое"
    /// </summary>
    /// <param name="id">Идентификатор животного</param>
    /// <returns>204 No Content при успешном обновлении или 404, если животное не найдено</returns>
    [HttpPut("{id}/health")]
    public async Task<IActionResult> UpdateAnimalHealth(Guid id)
    {
        var animal = await _animalRepository.GetByIdAsync(id);

        if (animal == null)
        {
            return NotFound();
        }

        animal.Heal();
        await _animalRepository.UpdateAsync(animal);

        return NoContent();
    }

    /// <summary>
    /// Перемещает животное из текущего вольера в указанный
    /// </summary>
    /// <param name="id">Идентификатор животного</param>
    /// <param name="enclosureId">Идентификатор целевого вольера</param>
    /// <returns>204 No Content при успешном перемещении или 400 Bad Request при ошибке</returns>
    [HttpPut("{id}/transfer/{enclosureId}")]
    public async Task<IActionResult> TransferAnimal(Guid id, Guid enclosureId)
    {
        var success = await _transferService.TransferAnimalAsync(id, enclosureId);

        if (!success)
        {
            return BadRequest("Не удалось переместить животное. Животное или вольер не найдены, либо вольер заполнен до максимальной вместимости.");
        }

        return NoContent();
    }

    /// <summary>
    /// Удаляет животное из системы
    /// </summary>
    /// <param name="id">Идентификатор животного</param>
    /// <returns>204 No Content при успешном удалении или 404, если животное не найдено</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(Guid id)
    {
        var animal = await _animalRepository.GetByIdAsync(id);
        if (animal == null)
        {
            return NotFound();
        }

        await _animalRepository.DeleteAsync(id);

        return NoContent();
    }
}

// DTO для создания животного
public class AnimalCreateRequest
{
    public string Species { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public Gender Gender { get; set; }
    public string FavFood { get; set; } = string.Empty;
    public HealthStatus HealthStatus { get; set; } = HealthStatus.Healthy;
}