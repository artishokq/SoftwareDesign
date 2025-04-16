namespace ZooManagerWeb.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using ZooManagerWeb.Infrastructure.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    public StatisticsController(
        IAnimalRepository animalRepository,
        IEnclosureRepository enclosureRepository,
        IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    /// <summary>
    /// Получает базовую статистику зоопарка: количество животных, вольеров и их заполненность
    /// </summary>
    /// <returns>Основные статистические данные о зоопарке</returns>
    [HttpGet]
    public async Task<ActionResult<ZooStatistics>> GetBasicStatistics()
    {
        var animals = await _animalRepository.GetAllAsync();
        var enclosures = await _enclosureRepository.GetAllAsync();
        
        int totalAnimals = animals.Count();
        int totalEnclosures = enclosures.Count();
        int freeEnclosures = enclosures.Count(e => e.CurrentAnimalNumber == 0);
        int occupiedEnclosures = totalEnclosures - freeEnclosures;
        
        var statistics = new ZooStatistics
        {
            TotalAnimals = totalAnimals,
            TotalEnclosures = totalEnclosures,
            FreeEnclosures = freeEnclosures,
            OccupiedEnclosures = occupiedEnclosures,
            AverageEnclosureOccupancy = totalEnclosures > 0 
                ? Math.Round((double)totalAnimals / totalEnclosures, 2) 
                : 0
        };
        
        return Ok(statistics);
    }
    
    /// <summary>
    /// Получает детальную статистику зоопарка, включая распределение животных по видам,
    /// статистику по типам вольеров и данные о кормлениях
    /// </summary>
    /// <returns>Расширенные статистические данные о зоопарке</returns>
    [HttpGet("detailed")]
    public async Task<ActionResult<DetailedZooStatistics>> GetDetailedStatistics()
    {
        var animals = await _animalRepository.GetAllAsync();
        var enclosures = await _enclosureRepository.GetAllAsync();
        var feedings = await _feedingScheduleRepository.GetAllAsync();
        
        int totalAnimals = animals.Count();
        int totalEnclosures = enclosures.Count();
        int freeEnclosures = enclosures.Count(e => e.CurrentAnimalNumber == 0);
        
        var speciesCount = animals
            .GroupBy(a => a.Species)
            .Select(g => new { Species = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToDictionary(x => x.Species, x => x.Count);
        
        var enclosureTypeCount = enclosures
            .GroupBy(e => e.Type)
            .Select(g => new { Type = g.Key.ToString(), Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToDictionary(x => x.Type, x => x.Count);
        
        int totalFeedingSchedules = feedings.Count();
        int completedFeedings = feedings.Count(f => f.FeedingStatus);
        int pendingFeedings = totalFeedingSchedules - completedFeedings;
        
        var detailedStatistics = new DetailedZooStatistics
        {
            BasicStatistics = new ZooStatistics
            {
                TotalAnimals = totalAnimals,
                TotalEnclosures = totalEnclosures,
                FreeEnclosures = freeEnclosures,
                OccupiedEnclosures = totalEnclosures - freeEnclosures,
                AverageEnclosureOccupancy = totalEnclosures > 0 
                    ? Math.Round((double)totalAnimals / totalEnclosures, 2) 
                    : 0
            },
            SpeciesDistribution = speciesCount,
            EnclosureTypeDistribution = enclosureTypeCount,
            FeedingStatistics = new FeedingStatistics
            {
                TotalSchedules = totalFeedingSchedules,
                CompletedFeedings = completedFeedings,
                PendingFeedings = pendingFeedings,
                CompletionRate = totalFeedingSchedules > 0 
                    ? Math.Round((double)completedFeedings / totalFeedingSchedules * 100, 1) 
                    : 0
            }
        };
        
        return Ok(detailedStatistics);
    }
}

/// <summary>
/// Модель для базовой статистики зоопарка
/// </summary>
public class ZooStatistics
{
    /// <summary>
    /// Общее количество животных в зоопарке
    /// </summary>
    public int TotalAnimals { get; set; }
    
    /// <summary>
    /// Общее количество вольеров в зоопарке
    /// </summary>
    public int TotalEnclosures { get; set; }
    
    /// <summary>
    /// Количество пустых вольеров
    /// </summary>
    public int FreeEnclosures { get; set; }
    
    /// <summary>
    /// Количество занятых вольеров
    /// </summary>
    public int OccupiedEnclosures { get; set; }
    
    /// <summary>
    /// Среднее количество животных на один вольер
    /// </summary>
    public double AverageEnclosureOccupancy { get; set; }
}

/// <summary>
/// Модель для статистики по кормлениям
/// </summary>
public class FeedingStatistics
{
    /// <summary>
    /// Общее количество запланированных кормлений
    /// </summary>
    public int TotalSchedules { get; set; }
    
    /// <summary>
    /// Количество выполненных кормлений
    /// </summary>
    public int CompletedFeedings { get; set; }
    
    /// <summary>
    /// Количество невыполненных кормлений
    /// </summary>
    public int PendingFeedings { get; set; }
    
    /// <summary>
    /// Процент выполненных кормлений от общего числа
    /// </summary>
    public double CompletionRate { get; set; } // в процентах
}

/// <summary>
/// Модель для расширенной статистики зоопарка
/// </summary>
public class DetailedZooStatistics
{
    /// <summary>
    /// Базовая статистика зоопарка
    /// </summary>
    public ZooStatistics BasicStatistics { get; set; } = new ZooStatistics();
    
    /// <summary>
    /// Распределение животных по видам (вид -> количество)
    /// </summary>
    public Dictionary<string, int> SpeciesDistribution { get; set; } = new Dictionary<string, int>();
    
    /// <summary>
    /// Распределение вольеров по типам (тип вольера -> количество)
    /// </summary>
    public Dictionary<string, int> EnclosureTypeDistribution { get; set; } = new Dictionary<string, int>();
    
    /// <summary>
    /// Статистика по кормлениям
    /// </summary>
    public FeedingStatistics FeedingStatistics { get; set; } = new FeedingStatistics();
}