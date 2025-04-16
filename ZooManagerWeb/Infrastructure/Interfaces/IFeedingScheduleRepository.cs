namespace ZooManagerWeb.Infrastructure.Interfaces;

using ZooManagerWeb.Domain.Entities;

public interface IFeedingScheduleRepository
{
    Task<FeedingSchedule?> GetByIdAsync(Guid id);
    Task<IEnumerable<FeedingSchedule>> GetAllAsync();
    Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId);
    Task<IEnumerable<FeedingSchedule>> GetUpcomingSchedulesAsync(DateTime fromDate, DateTime toDate);
    Task AddAsync(FeedingSchedule schedule);
    Task UpdateAsync(FeedingSchedule schedule);
    Task DeleteAsync(Guid id);
}