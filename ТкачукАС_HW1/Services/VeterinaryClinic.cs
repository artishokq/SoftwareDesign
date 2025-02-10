using ТкачукАС_HW1.Interfaces;
using ТкачукАС_HW1.Models;

namespace ТкачукАС_HW1.Services;

/// <summary>
/// Класс ветеринарной клиники
/// </summary>
public class VeterinaryClinic : IVeterinaryClinic
{
    public bool CheckHealth(Animal animal)
    {
        return animal.IsHealthy;
    }
}