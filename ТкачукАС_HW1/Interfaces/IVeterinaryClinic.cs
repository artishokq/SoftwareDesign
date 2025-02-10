using ТкачукАС_HW1.Models;

namespace ТкачукАС_HW1.Interfaces;

/// <summary>
/// Интерфейс ветеринарной клиники, отвечающей за проверку здоровья животных
/// </summary>
public interface IVeterinaryClinic
{
    bool CheckHealth(Animal animal);
}