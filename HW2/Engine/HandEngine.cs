namespace HW2.Engine;

using HW2.Car;
using HW2.Customer;
using HW2.Service;

public class HandEngine : IEngine
{
    public EngineType Type => EngineType.Hand;

    public bool IsCompatibleWith(Customer customer)
    {
        return customer.HandStrength > 10;
    }
    
    public override string ToString()
    {
        return $"Тип: {Type}";
    }
}