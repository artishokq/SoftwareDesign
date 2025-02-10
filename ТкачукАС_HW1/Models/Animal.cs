using ТкачукАС_HW1.Interfaces;

namespace ТкачукАС_HW1.Models;

/// <summary>
/// Абстрактный класс животного, реализующий IAlive
/// </summary>
public abstract class Animal : IAlive
{
    // Имя животного
    public string Name { get; protected set; }
    // Суточное потребление еды (кг)
    public int Food { get; set; }
    
    // Флаг, указывающий на состояние здоровья, проверяется ветеринарос
    public bool IsHealthy { get; set; }

    protected Animal(string name, int food)
    {
        Name = name;
        Food = food;
    }
}