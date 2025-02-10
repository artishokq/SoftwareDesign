using ТкачукАС_HW1.Interfaces;

namespace ТкачукАС_HW1.Models;

/// <summary>
/// Абстрактный базовый класс для всех инвентаризационных объектов
/// </summary>
public abstract class InventoryItem : IInventory
{
    public int Number { get; set; }
    public string Name { get; protected set; }

    protected InventoryItem(int number, string name)
    {
        Number = number;
        Name = name;
    }
}