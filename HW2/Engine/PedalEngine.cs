namespace HW2.Engine;

using HW2.Car;
using HW2.Customer;
using HW2.Service;

public class PedalEngine : IEngine
{
    private int Size { get; }

    public EngineType Type => EngineType.Pedal;

    public PedalEngine(int size)
    {
        Size = size;
    }
    
    public bool IsCompatibleWith(Customer customer)
    {
        return customer.LegStrength > 10;
    }

    public override string ToString()
    {
        return $"Тип: {Type}, Размер педалей: {Size}";
    }
}