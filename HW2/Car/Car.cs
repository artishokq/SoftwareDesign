namespace HW2.Car;

using HW2.Customer;
using HW2.Engine;
using HW2.Service;

public class Car
{
    public int Number { get; }
    public IEngine Engine { get; }

    public Car(IEngine engine, int number)
    {
        Engine = engine;
        Number = number;
    }
    
    public bool IsCompatibleWith(Customer customer)
    {
        return Engine.IsCompatibleWith(customer);
    }

    public override string ToString()
    {
        return $"Номер: {Number}, Двигатель: {{ {Engine} }}";
    }
}