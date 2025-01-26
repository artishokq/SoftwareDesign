namespace HW2.Engine;

using HW2.Car;
using HW2.Customer;
using HW2.Service;

public enum EngineType
{
    Pedal,
    Hand,
}

public interface IEngine
{
    EngineType Type { get; }

    bool IsCompatibleWith(Customer customer);
}