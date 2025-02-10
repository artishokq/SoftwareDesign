namespace ТкачукАС_HW1.Interfaces;

/// <summary>
/// Интерфейс для живых объектов
/// </summary>
public interface IAlive
{
    string Name { get; }
    int Food { get; set; }
}