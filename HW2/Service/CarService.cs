namespace HW2.Service;

using HW2.Car;
using HW2.Customer;
using HW2.Engine;

public class CarService : ICarProvider
{
    private readonly List<Car> _cars = new();
    private readonly HashSet<Car> _assignedCars = new();
    private int _nextCarNumber = 1;

    public Car? FindSuitableCar(Customer customer)
    {
        var car = _cars.FirstOrDefault(c => !_assignedCars.Contains(c) && c.IsCompatibleWith(customer));
        if (car != null)
        {
            _assignedCars.Add(car);
        }
        return car;
    }

    public void AddCar<TParams>(ICarFactory<TParams> factory, TParams parameters)
    {
        var car = factory.CreateCar(parameters, _nextCarNumber++);
        _cars.Add(car);
    }
}