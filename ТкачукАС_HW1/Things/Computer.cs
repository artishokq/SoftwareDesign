namespace ТкачукАС_HW1.Things;

/// <summary>
/// Класс компьютера
/// </summary>
public class Computer : Thing
{
    public Computer(int number, string name) : base(number, name)
    { }

    public override string ToString()
    {
        return $"{Name} (Компьютер) - Инвентарь #{Number}";
    }
}