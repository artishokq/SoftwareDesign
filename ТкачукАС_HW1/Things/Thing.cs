using ТкачукАС_HW1.Models;

namespace ТкачукАС_HW1.Things;

/// <summary>
/// Абстрактный класс вещи, подлежащей инвентаризации
/// </summary>
public abstract class Thing : InventoryItem
{
    protected Thing(int number, string name) : base(number, name)
    { }
}