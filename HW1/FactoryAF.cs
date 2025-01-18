namespace HW1;

public class FactoryAF
{
    public List<Car> Cars { get; }
    public List<Customer> Customers { get; }

    public FactoryAF()
    {
        Cars = new List<Car>();
        Customers = new List<Customer>();
    }

    public void AddCar()
    {
        Cars.Add(new Car(Cars.Count + 1));
    }

    public void AddCustomer(Customer customer)
    {
        Customers.Add(customer);
    }

    public void SaleCar()
    {
        var customersWithoutCar = Customers.Where(c => c.Car == null).ToList();
        for (int i = 0; i < customersWithoutCar.Count && i < Cars.Count; i++)
        {
            var customer = customersWithoutCar[i];
            var car = Cars[i];
            customer.Car = car;
            Console.WriteLine($"Автомобиль #{car.Number} продан клиенту {customer.Name}");
        }
        
        if (customersWithoutCar.Count > Cars.Count)
        {
            foreach (var customer in customersWithoutCar.Skip(Cars.Count))
            {
                Console.WriteLine($"Для клиента {customer.Name} не хватило автомобиля");
            }
        }
        
        if (Cars.Count > customersWithoutCar.Count)
        {
            Console.WriteLine($"\nОставшиеся {Cars.Count - customersWithoutCar.Count} автомобилей ликвидированы");
        }

        Cars.Clear();
    }
}