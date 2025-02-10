namespace ТкачукАС_HW1.Models;

/// <summary>
/// Класс обезьяны. Не является ни травоядной, ни чистым хищником – базовый тип
/// </summary>
public class Monkey : Animal
{
    public Monkey(string name, int food) : base(name, food)
    { }

    public override string ToString()
    {
        return $"{Name} (Абизяна), Еда: {Food} кг";
    }
}