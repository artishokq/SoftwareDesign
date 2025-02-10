namespace ТкачукАС_HW1.Models;

/// <summary>
/// Класс волка – наследник Predator
/// </summary>
public class Wolf : Predator
{
    public Wolf(string name, int food) : base(name, food)
    { }

    public override string ToString()
    {
        return $"{Name} (Волк), Еда: {Food} кг";
    }
}