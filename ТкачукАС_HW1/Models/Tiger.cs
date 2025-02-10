namespace ТкачукАС_HW1.Models;

/// <summary>
/// Класс тигра – наследник Predator
/// </summary>
public class Tiger : Predator
{
    public Tiger(string name, int food) : base(name, food)
    { }

    public override string ToString()
    {
        return $"{Name} (Тигр), Еда: {Food} кг";
    }
}