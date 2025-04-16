namespace ZooManagerWeb.Application.Services;

using ZooManagerWeb.Domain.Value_Object.Animal;
using ZooManagerWeb.Infrastructure.Interfaces;

public class ZooStatisticsService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    public ZooStatisticsService(
        IAnimalRepository animalRepository,
        IEnclosureRepository enclosureRepository,
        IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    // Получение общего количества животных
    public async Task<int> GetTotalAnimalCountAsync()
    {
        var animals = await _animalRepository.GetAllAsync();
        return animals.Count();
    }

    // Получение количества животных по видам
    public async Task<Dictionary<string, int>> GetAnimalCountBySpeciesAsync()
    {
        var animals = await _animalRepository.GetAllAsync();
        return animals
            .GroupBy(a => a.Species)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    // Получение соотношения полов
    public async Task<Dictionary<Gender, int>> GetGenderDistributionAsync()
    {
        var animals = await _animalRepository.GetAllAsync();
        return animals
            .GroupBy(a => a.Gender)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    // Получение статистики по здоровью
    public async Task<Dictionary<HealthStatus, int>> GetHealthStatusDistributionAsync()
    {
        var animals = await _animalRepository.GetAllAsync();
        return animals
            .GroupBy(a => a.HealthStatus)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    // Получение статистики по возрастным группам
    public async Task<Dictionary<string, int>> GetAgeGroupDistributionAsync()
    {
        var animals = await _animalRepository.GetAllAsync();
        var now = DateTime.UtcNow;

        var ageGroups = new Dictionary<string, int>
        {
            { "Молодой (< 2 лет)", 0 },
            { "Взрослый (2-10 лет)", 0 },
            { "Старый (> 10 лет)", 0 }
        };

        foreach (var animal in animals)
        {
            var age = now.Year - animal.Birthday.Year;
            if (now.DayOfYear < animal.Birthday.DayOfYear)
            {
                age--;
            }

            if (age < 2)
            {
                ageGroups["Молодой (< 2 лет)"]++;
            }
            else if (age <= 10)
            {
                ageGroups["Взрослый (2-10 лет)"]++;
            }
            else
            {
                ageGroups["Старый (> 10 лет)"]++;
            }
        }

        return ageGroups;
    }

    // Получение статистики по заполненности вольеров
    public async Task<Dictionary<string, object>> GetEnclosureStatisticsAsync()
    {
        var enclosures = await _enclosureRepository.GetAllAsync();
        var result = new Dictionary<string, object>();

        result.Add("TotalEnclosures", enclosures.Count());
        result.Add("TotalCapacity", enclosures.Sum(e => e.Capacity));
        result.Add("OccupiedCapacity", enclosures.Sum(e => e.CurrentAnimalNumber));

        var enclosuresByType = enclosures
            .GroupBy(e => e.Type)
            .ToDictionary(g => g.Key.ToString(), g => g.Count());
        result.Add("EnclosuresByType", enclosuresByType);

        var occupancyRates = enclosures
            .ToDictionary(
                e => e.Id.ToString(),
                e => Math.Round((double)e.CurrentAnimalNumber / e.Capacity * 100, 1)
            );
        result.Add("OccupancyRates", occupancyRates);

        return result;
    }

    // Получение статистики по кормлениям
    public async Task<Dictionary<string, object>> GetFeedingStatisticsAsync(DateTime fromDate, DateTime toDate)
    {
        var schedules = await _feedingScheduleRepository.GetAllAsync();
        var filteredSchedules = schedules.Where(s => s.FeedingTime >= fromDate && s.FeedingTime <= toDate);

        var result = new Dictionary<string, object>();

        var totalCount = filteredSchedules.Count();
        var completedCount = filteredSchedules.Count(s => s.FeedingStatus);

        result.Add("TotalSchedules", totalCount);
        result.Add("CompletedFeedings", completedCount);
        result.Add("PendingFeedings", totalCount - completedCount);
        result.Add("CompletionRate", totalCount > 0
            ? Math.Round((double)completedCount / totalCount * 100, 1)
            : 0);

        var feedingsByDayOfWeek = filteredSchedules
            .GroupBy(s => s.FeedingTime.DayOfWeek)
            .ToDictionary(g => g.Key.ToString(), g => g.Count());
        result.Add("FeedingsByDayOfWeek", feedingsByDayOfWeek);

        return result;
    }
}