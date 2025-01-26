namespace HW2.Car;

using HW2.Customer;
using HW2.Engine;
using HW2.Service;

public interface ICarProvider
{
    Car? FindSuitableCar(Customer customer);
}