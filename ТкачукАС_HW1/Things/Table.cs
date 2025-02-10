namespace ТкачукАС_HW1.Things;

/// <summary>
/// Класс стола
/// </summary>
public class Table : Thing
{
    public Table(int number, string name) : base(number, name)
    { }

    public override string ToString()
    {
        return $"{Name} (Стол) - Инвентарь #{Number}";
    }
}