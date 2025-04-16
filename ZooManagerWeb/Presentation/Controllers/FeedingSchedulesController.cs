namespace ZooManagerWeb.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Infrastructure.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class FeedingSchedulesController : ControllerBase
{
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;
    private readonly IAnimalRepository _animalRepository;

    public FeedingSchedulesController(
        IFeedingScheduleRepository feedingScheduleRepository,
        IAnimalRepository animalRepository)
    {
        _feedingScheduleRepository = feedingScheduleRepository;
        _animalRepository = animalRepository;
    }

    /// <summary>
    /// Получает список всех расписаний кормления в зоопарке
    /// </summary>
    /// <returns>Коллекция всех расписаний кормления</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetFeedingSchedules()
    {
        var schedules = await _feedingScheduleRepository.GetAllAsync();
        return Ok(schedules);
    }

    /// <summary>
    /// Получает информацию о конкретном расписании кормления по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор расписания кормления</param>
    /// <returns>Данные о расписании или 404, если расписание не найдено</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<FeedingSchedule>> GetFeedingSchedule(Guid id)
    {
        var schedule = await _feedingScheduleRepository.GetByIdAsync(id);

        if (schedule == null)
        {
            return NotFound();
        }

        return Ok(schedule);
    }

    /// <summary>
    /// Получает все расписания кормления для конкретного животного
    /// </summary>
    /// <param name="animalId">Идентификатор животного</param>
    /// <returns>Коллекция расписаний кормления животного или 404, если животное не найдено</returns>
    [HttpGet("animal/{animalId}")]
    public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetSchedulesByAnimal(Guid animalId)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId);
        if (animal == null)
        {
            return NotFound("Животное не найдено");
        }

        var schedules = await _feedingScheduleRepository.GetByAnimalIdAsync(animalId);
        return Ok(schedules);
    }

    /// <summary>
    /// Получает список предстоящих кормлений в указанном временном диапазоне
    /// </summary>
    /// <param name="from">Начальная дата/время (по умолчанию текущий момент)</param>
    /// <param name="to">Конечная дата/время (по умолчанию +1 день от начальной даты)</param>
    /// <returns>Коллекция предстоящих расписаний кормления</returns>
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetUpcomingSchedules([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        DateTime fromDate = from ?? DateTime.UtcNow;
        DateTime toDate = to ?? fromDate.AddDays(1);

        var schedules = await _feedingScheduleRepository.GetUpcomingSchedulesAsync(fromDate, toDate);
        return Ok(schedules);
    }

    /// <summary>
    /// Создает новое расписание кормления в системе
    /// </summary>
    /// <param name="request">Данные для создания расписания кормления</param>
    /// <returns>Созданное расписание с присвоенным идентификатором или 404, если животное не найдено</returns>
    [HttpPost]
    public async Task<ActionResult<FeedingSchedule>> CreateFeedingSchedule(FeedingScheduleCreateRequest request)
    {
        var animal = await _animalRepository.GetByIdAsync(request.AnimalId);
        if (animal == null)
        {
            return NotFound("Животное не найдено");
        }

        var schedule = new FeedingSchedule(
            request.AnimalId,
            request.FoodType,
            request.FeedingTime
        );

        await _feedingScheduleRepository.AddAsync(schedule);

        return CreatedAtAction(nameof(GetFeedingSchedule), new { id = schedule.Id }, schedule);
    }

    /// <summary>
    /// Обновляет существующее расписание кормления
    /// </summary>
    /// <param name="id">Идентификатор расписания кормления</param>
    /// <param name="request">Обновленные данные расписания</param>
    /// <returns>204 No Content при успешном обновлении или 404, если расписание не найдено</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeedingSchedule(Guid id, FeedingScheduleUpdateRequest request)
    {
        var schedule = await _feedingScheduleRepository.GetByIdAsync(id);
        if (schedule == null)
        {
            return NotFound();
        }

        schedule.UpdateSchedule(request.FoodType, request.FeedingTime);
        await _feedingScheduleRepository.UpdateAsync(schedule);

        return NoContent();
    }

    /// <summary>
    /// Отмечает кормление как выполненное и вызывает метод Feed() у соответствующего животного
    /// </summary>
    /// <param name="id">Идентификатор расписания кормления</param>
    /// <returns>204 No Content при успешном выполнении или 404, если расписание не найдено</returns>
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> MarkAsCompleted(Guid id)
    {
        var schedule = await _feedingScheduleRepository.GetByIdAsync(id);
        if (schedule == null)
        {
            return NotFound();
        }

        schedule.isCompleted();
        
        var animal = await _animalRepository.GetByIdAsync(schedule.AnimalId);
        if (animal != null)
        {
            animal.Feed();
            await _animalRepository.UpdateAsync(animal);
        }
        
        await _feedingScheduleRepository.UpdateAsync(schedule);

        return NoContent();
    }

    /// <summary>
    /// Сбрасывает статус кормления на "не выполнено"
    /// </summary>
    /// <param name="id">Идентификатор расписания кормления</param>
    /// <returns>204 No Content при успешном сбросе или 404, если расписание не найдено</returns>
    [HttpPut("{id}/reset")]
    public async Task<IActionResult> ResetFeedingStatus(Guid id)
    {
        var schedule = await _feedingScheduleRepository.GetByIdAsync(id);
        if (schedule == null)
        {
            return NotFound();
        }

        schedule.ResetFeedingStatus();
        await _feedingScheduleRepository.UpdateAsync(schedule);

        return NoContent();
    }

    /// <summary>
    /// Удаляет расписание кормления из системы
    /// </summary>
    /// <param name="id">Идентификатор расписания кормления</param>
    /// <returns>204 No Content при успешном удалении или 404, если расписание не найдено</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeedingSchedule(Guid id)
    {
        var schedule = await _feedingScheduleRepository.GetByIdAsync(id);
        if (schedule == null)
        {
            return NotFound();
        }

        await _feedingScheduleRepository.DeleteAsync(id);

        return NoContent();
    }
}

// DTO для создания расписания кормления
public class FeedingScheduleCreateRequest
{
    public Guid AnimalId { get; set; }
    public string FoodType { get; set; } = string.Empty;
    public DateTime FeedingTime { get; set; }
}

// DTO для обновления расписания кормления
public class FeedingScheduleUpdateRequest
{
    public string FoodType { get; set; } = string.Empty;
    public DateTime FeedingTime { get; set; }
}