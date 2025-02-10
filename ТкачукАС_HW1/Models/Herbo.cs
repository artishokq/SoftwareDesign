namespace ТкачукАС_HW1.Models;

/// <summary>
/// Класс травоядного животного. Дополнительно хранит информацию об уровне доброты
/// </summary>
public class Herbo : Animal
{
    // Уровень доброты (от 0 до 10). Если > 5 – животное можно использовать для контактного зоопарка
    public int Kindness { get; set; }

    public Herbo(string name, int food, int kindness) : base(name, food)
    {
        Kindness = kindness;
    }

    public override string ToString()
    {
        return $"{Name} (Травоядный), Еда: {Food} кг, Доброта: {Kindness}";
    }
}