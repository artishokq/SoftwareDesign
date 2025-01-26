namespace HW2.Customer;

using HW2.Car;
using HW2.Engine;
using HW2.Service;

public class Customer
{
    private string Name { get; }
    public int HandStrength { get; }
    public int LegStrength { get; }
    public Car? Car { get; set; }

    public Customer(string name, int legStrength, int handStrength)
    {
        Name = name;
        LegStrength = legStrength;
        HandStrength = handStrength;
    }
    
    public override string ToString()
    {
        return $"Имя: {Name}, Сила ног: {LegStrength}, Сила рук: {HandStrength}, " +
               $"Машина: {(Car == null ? "нет" : $"есть (номер {Car.Number}, тип двигателя: {Car.Engine})")}";
    }
}