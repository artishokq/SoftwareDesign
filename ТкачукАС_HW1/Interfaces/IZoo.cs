using ТкачукАС_HW1.Models;
using ТкачукАС_HW1.Things;

namespace ТкачукАС_HW1.Interfaces;

/// <summary>
/// Интерфейс зоопарка
/// </summary>
public interface IZoo
{
    // Добавление животного в зоопарк (с предварительной проверкой здоровья)
    void AddAnimal(Animal animal);
    
    // Добавление вещи в инвентарь зоопарка
    void AddThing(Thing thing);
    
    // Суммарное потребление еды животными (в кг)
    int TotalFoodConsumption { get; }
    
    // Список животных, находящихся на балансе зоопарка
    IEnumerable<Animal> GetAnimals();
    
    // Список животных, пригодных для контактного зоопарка (травоядные с уровнем доброты > 5)
    IEnumerable<Herbo> GetInteractiveAnimals();
    
    /// Список всех инвентаризационных объектов (животные и вещи)
    IEnumerable<IInventory> GetInventoryItems();
}