namespace ТкачукАС_HW1.Models;

/// <summary>
/// Класс хищного животного
/// </summary>
public class Predator : Animal
{
    public Predator(string name, int food) : base(name, food)
    { }

    public override string ToString()
    {
        return $"{Name} (Хищник), Еда: {Food} кг";
    }
}