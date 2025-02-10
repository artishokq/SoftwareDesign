using ТкачукАС_HW1.Interfaces;
using ТкачукАС_HW1.Models;
using ТкачукАС_HW1.Things;

namespace ТкачукАС_HW1.Services;

/// <summary>
/// Класс зоопарка
/// </summary>
public class Zoo : IZoo
{
    private readonly IVeterinaryClinic _vetClinic;
    private readonly List<Animal> _animals = new List<Animal>();
    private readonly List<Thing> _things = new List<Thing>();

    public Zoo(IVeterinaryClinic vetClinic)
    {
        _vetClinic = vetClinic;
    }

    public void AddAnimal(Animal animal)
    {
        if (_vetClinic.CheckHealth(animal))
        {
            _animals.Add(animal);
            Console.WriteLine($"{animal.Name} успешно принят в зоопарк.");
        }
        else
        {
            Console.WriteLine($"{animal.Name} отклонён ветеринарной клиникой (проблемы со здоровьем).");
        }
    }

    public void AddThing(Thing thing)
    {
        _things.Add(thing);
        Console.WriteLine($"{thing.Name} добавлен в инвентарь.");
    }

    public int TotalFoodConsumption => _animals.Sum(a => a.Food);

    public IEnumerable<Animal> GetAnimals() => _animals;

    public IEnumerable<Herbo> GetInteractiveAnimals()
    {
        // Отбираем травоядных, у которых уровень доброты больше 5
        return _animals.OfType<Herbo>().Where(h => h.Kindness > 5);
    }

    public IEnumerable<IInventory> GetInventoryItems()
    {
        // Инвентаризационные объекты
        return _things;
    }
}