namespace ZooManagerWeb.Infrastructure.Repositories;

using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Infrastructure.Interfaces;

public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly List<FeedingSchedule> _feedingSchedules = new List<FeedingSchedule>();

    public Task<FeedingSchedule?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_feedingSchedules.FirstOrDefault(f => f.Id == id));
    }

    public Task<IEnumerable<FeedingSchedule>> GetAllAsync()
    {
        return Task.FromResult(_feedingSchedules.AsEnumerable());
    }

    public Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId)
    {
        var result = _feedingSchedules.Where(f => f.AnimalId == animalId);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<FeedingSchedule>> GetUpcomingSchedulesAsync(DateTime fromDate, DateTime toDate)
    {
        var result = _feedingSchedules.Where(f =>
            f.FeedingTime >= fromDate &&
            f.FeedingTime <= toDate &&
            !f.FeedingStatus);

        return Task.FromResult(result);
    }

    public Task AddAsync(FeedingSchedule schedule)
    {
        _feedingSchedules.Add(schedule);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FeedingSchedule schedule)
    {
        var existingSchedule = _feedingSchedules.FirstOrDefault(f => f.Id == schedule.Id);
        if (existingSchedule != null)
        {
            _feedingSchedules.Remove(existingSchedule);
            _feedingSchedules.Add(schedule);
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var schedule = _feedingSchedules.FirstOrDefault(f => f.Id == id);
        if (schedule != null)
        {
            _feedingSchedules.Remove(schedule);
        }

        return Task.CompletedTask;
    }
}