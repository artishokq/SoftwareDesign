namespace HW2.Car;

using HW2.Customer;
using HW2.Engine;
using HW2.Service;

public class PedalCarFactory : ICarFactory<PedalEngineParams>
{
    public Car CreateCar(PedalEngineParams parameters, int number)
    {
        return new Car(new PedalEngine(parameters.PedalSize), number);
    }
}