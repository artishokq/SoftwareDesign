namespace HW2.Car;

using HW2.Customer;
using HW2.Engine;
using HW2.Service;

public class HandCarFactory : ICarFactory<EmptyEngineParams>
{
    public Car CreateCar(EmptyEngineParams parameters, int number)
    {
        return new Car(new HandEngine(), number);
    }
}