namespace ZooManagerWeb.Application.Services;

using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Domain.Events;
using ZooManagerWeb.Infrastructure.Interfaces;

public class FeedingOrganizationService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    // Делегат для публикации событий
    public delegate void EventHandler<T>(T eventArgs) where T : class;

    // Событие для подписки на время кормления
    public event EventHandler<FeedingTimeEvent> FeedingTime;

    public FeedingOrganizationService(
        IAnimalRepository animalRepository,
        IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    // Создание нового расписания кормления
    public async Task<FeedingSchedule> CreateFeedingScheduleAsync(
        Guid animalId, string foodType, DateTime feedingTime)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId);
        if (animal == null)
        {
            throw new ArgumentException("Animal not found", nameof(animalId));
        }

        var schedule = new FeedingSchedule(animalId, foodType, feedingTime);
        await _feedingScheduleRepository.AddAsync(schedule);

        return schedule;
    }

    // Обновление расписания кормления
    public async Task UpdateFeedingScheduleAsync(
        Guid scheduleId, string newFoodType, DateTime newFeedingTime)
    {
        var schedule = await _feedingScheduleRepository.GetByIdAsync(scheduleId);
        if (schedule == null)
        {
            throw new ArgumentException("Feeding schedule not found", nameof(scheduleId));
        }

        schedule.UpdateSchedule(newFoodType, newFeedingTime);
        await _feedingScheduleRepository.UpdateAsync(schedule);
    }

    // Отметка о выполнении кормления
    public async Task MarkFeedingAsCompletedAsync(Guid scheduleId)
    {
        var schedule = await _feedingScheduleRepository.GetByIdAsync(scheduleId);
        if (schedule == null)
        {
            throw new ArgumentException("Feeding schedule not found", nameof(scheduleId));
        }

        schedule.isCompleted();
        await _feedingScheduleRepository.UpdateAsync(schedule);

        var animal = await _animalRepository.GetByIdAsync(schedule.AnimalId);
        if (animal != null)
        {
            animal.Feed();
            await _animalRepository.UpdateAsync(animal);
        }
    }

    // Проверка предстоящих кормлений и генерация событий
    public async Task CheckUpcomingFeedingsAsync()
    {
        var now = DateTime.UtcNow;
        var upcoming = await _feedingScheduleRepository.GetUpcomingSchedulesAsync(
            now, now.AddMinutes(15));

        foreach (var schedule in upcoming)
        {
            // Создаем и публикуем событие о времени кормления
            var feedingEvent = new FeedingTimeEvent(
                schedule.Id, schedule.AnimalId, schedule.FoodType, schedule.FeedingTime);
            OnFeedingTime(feedingEvent);
        }
    }

    // Метод для вызова события
    protected virtual void OnFeedingTime(FeedingTimeEvent e)
    {
        FeedingTime?.Invoke(e);
    }
}