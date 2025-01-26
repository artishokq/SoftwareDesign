namespace HW2.Car;

using HW2.Customer;
using HW2.Engine;
using HW2.Service;

public interface ICarFactory<TParams>
{
    Car CreateCar(TParams parameters, int number);
}