namespace HW2.Service;

using HW2.Car;
using HW2.Customer;
using HW2.Engine;

public class HseCarService
{
    private readonly ICarProvider _carProvider;
    private readonly ICustomersProvider _customersProvider;

    public HseCarService(ICarProvider carProvider, ICustomersProvider customersProvider)
    {
        _carProvider = carProvider;
        _customersProvider = customersProvider;
    }

    public void SellCars()
    {
        var customers = _customersProvider.GetCustomers().ToList();
        foreach (var customer in customers)
        {
            if (customer.Car == null)
            {
                var car = _carProvider.FindSuitableCar(customer);
                if (car != null)
                {
                    customer.Car = car;
                }
            }
        }
    }
}