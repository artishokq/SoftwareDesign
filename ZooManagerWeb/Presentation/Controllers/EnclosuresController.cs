namespace ZooManagerWeb.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Infrastructure.Interfaces;
using ZooManagerWeb.Domain.Value_Object.Enclosure;

[ApiController]
[Route("api/[controller]")]
public class EnclosuresController : ControllerBase
{
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IAnimalRepository _animalRepository;

    public EnclosuresController(
        IEnclosureRepository enclosureRepository,
        IAnimalRepository animalRepository)
    {
        _enclosureRepository = enclosureRepository;
        _animalRepository = animalRepository;
    }

    /// <summary>
    /// Получает список всех вольеров в зоопарке с информацией о находящихся в них животных
    /// </summary>
    /// <returns>Коллекция вольеров с подробной информацией</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnclosureResponse>>> GetEnclosures()
    {
        var enclosures = await _enclosureRepository.GetAllAsync();
        var response = new List<EnclosureResponse>();

        foreach (var enclosure in enclosures)
        {
            var animals = await _animalRepository.GetByEnclosureIdAsync(enclosure.Id);
            response.Add(new EnclosureResponse
            {
                Id = enclosure.Id,
                Type = enclosure.Type,
                Size = enclosure.Size,
                Capacity = enclosure.Capacity,
                CurrentAnimalNumber = enclosure.CurrentAnimalNumber,
                AnimalIds = enclosure.AnimalIds.ToList()
            });
        }

        return Ok(response);
    }

    /// <summary>
    /// Получает информацию о конкретном вольере по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор вольера</param>
    /// <returns>Данные о вольере или 404, если вольер не найден</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<EnclosureResponse>> GetEnclosure(Guid id)
    {
        var enclosure = await _enclosureRepository.GetByIdAsync(id);

        if (enclosure == null)
        {
            return NotFound();
        }

        var animals = await _animalRepository.GetByEnclosureIdAsync(enclosure.Id);
        var response = new EnclosureResponse
        {
            Id = enclosure.Id,
            Type = enclosure.Type,
            Size = enclosure.Size,
            Capacity = enclosure.Capacity,
            CurrentAnimalNumber = enclosure.CurrentAnimalNumber,
            AnimalIds = enclosure.AnimalIds.ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Создает новый вольер в системе
    /// </summary>
    /// <param name="request">Данные для создания вольера (тип, размер, вместимость)</param>
    /// <returns>Созданный вольер с присвоенным идентификатором</returns>
    [HttpPost]
    public async Task<ActionResult<Enclosure>> CreateEnclosure(EnclosureCreateRequest request)
    {
        var enclosure = new Enclosure(
            request.Type,
            request.Size,
            request.Capacity
        );

        await _enclosureRepository.AddAsync(enclosure);

        return CreatedAtAction(nameof(GetEnclosure), new { id = enclosure.Id }, enclosure);
    }

    /// <summary>
    /// Добавляет животное в указанный вольер
    /// </summary>
    /// <param name="id">Идентификатор вольера</param>
    /// <param name="animalId">Идентификатор животного</param>
    /// <returns>204 No Content при успешном добавлении или соответствующий код ошибки</returns>
    [HttpPut("{id}/animal/{animalId}")]
    public async Task<IActionResult> AddAnimalToEnclosure(Guid id, Guid animalId)
    {
        var enclosure = await _enclosureRepository.GetByIdAsync(id);
        if (enclosure == null)
        {
            return NotFound("Вольер не найден");
        }

        var animal = await _animalRepository.GetByIdAsync(animalId);
        if (animal == null)
        {
            return NotFound("Животное не найдено");
        }
        
        if (animal.CurrentEnclosureId != null)
        {
            return BadRequest("Животное уже находится в вольере");
        }
        
        if (!enclosure.AddAnimal(animalId))
        {
            return BadRequest("Вольер заполнен до максимальной вместимости");
        }
        
        animal.UpdateEnclosure(id);

        await _enclosureRepository.UpdateAsync(enclosure);
        await _animalRepository.UpdateAsync(animal);

        return NoContent();
    }

    /// <summary>
    /// Удаляет животное из указанного вольера
    /// </summary>
    /// <param name="id">Идентификатор вольера</param>
    /// <param name="animalId">Идентификатор животного</param>
    /// <returns>204 No Content при успешном удалении или соответствующий код ошибки</returns>
    [HttpDelete("{id}/animal/{animalId}")]
    public async Task<IActionResult> RemoveAnimalFromEnclosure(Guid id, Guid animalId)
    {
        var enclosure = await _enclosureRepository.GetByIdAsync(id);
        if (enclosure == null)
        {
            return NotFound("Вольер не найден");
        }

        var animal = await _animalRepository.GetByIdAsync(animalId);
        if (animal == null)
        {
            return NotFound("Животное не найдено");
        }
        
        if (animal.CurrentEnclosureId != id)
        {
            return BadRequest("Животное не находится в этом вольере");
        }
        
        if (!enclosure.DeleteAnimal(animalId))
        {
            return BadRequest("Не удалось удалить животное из вольера");
        }
        
        animal.UpdateEnclosure(null);

        await _enclosureRepository.UpdateAsync(enclosure);
        await _animalRepository.UpdateAsync(animal);

        return NoContent();
    }

    /// <summary>
    /// Удаляет вольер из системы
    /// </summary>
    /// <param name="id">Идентификатор вольера</param>
    /// <returns>204 No Content при успешном удалении или соответствующий код ошибки</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEnclosure(Guid id)
    {
        var enclosure = await _enclosureRepository.GetByIdAsync(id);
        if (enclosure == null)
        {
            return NotFound();
        }
        
        if (enclosure.CurrentAnimalNumber > 0)
        {
            return BadRequest("Невозможно удалить вольер с животными. Сначала удалите всех животных.");
        }

        await _enclosureRepository.DeleteAsync(id);

        return NoContent();
    }
}

// DTO для создания вольера
public class EnclosureCreateRequest
{
    public EnclosureType Type { get; set; }
    public int Size { get; set; }
    public int Capacity { get; set; }
}

// DTO для ответа с вольером
public class EnclosureResponse
{
    public Guid Id { get; set; }
    public EnclosureType Type { get; set; }
    public int Size { get; set; }
    public int Capacity { get; set; }
    public int CurrentAnimalNumber { get; set; }
    public List<Guid> AnimalIds { get; set; } = new List<Guid>();
}