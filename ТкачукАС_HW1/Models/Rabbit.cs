namespace ТкачукАС_HW1.Models;

/// <summary>
/// Класс кролика – наследник Herbo
/// </summary>
public class Rabbit : Herbo
{
    public Rabbit(string name, int food, int kindness) : base(name, food, kindness)
    { }

    public override string ToString()
    {
        return $"{Name} (Кролик), Еда: {Food} кг, Доброта: {Kindness}";
    }
}