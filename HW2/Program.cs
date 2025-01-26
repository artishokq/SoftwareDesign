namespace HW2;

using HW2.Car;
using HW2.Customer;
using HW2.Engine;
using HW2.Service;

public class Program
{
    public static void Main()
    {
        // Создаём services
        var carService = new CarService();
        var customerStorage = new CustomerStorage();
        var hseCarService = new HseCarService(carService, customerStorage);

        // Создаём factories
        var pedalFactory = new PedalCarFactory();
        var handFactory = new HandCarFactory();

        // Добавляем customers
        customerStorage.AddCustomer(new Customer.Customer("Клиент 1", 14, 4));
        customerStorage.AddCustomer(new Customer.Customer("Клиент 2", 15, 17));
        customerStorage.AddCustomer(new Customer.Customer("Клиент 3", 4, 5));
        customerStorage.AddCustomer(new Customer.Customer("Клиент 4", 20, 20));

        // Добавляем cars
        carService.AddCar(pedalFactory, new PedalEngineParams { PedalSize = 5 });
        carService.AddCar(pedalFactory, new PedalEngineParams { PedalSize = 6 });
        carService.AddCar(handFactory, EmptyEngineParams.DEFAULT);
        carService.AddCar(handFactory, EmptyEngineParams.DEFAULT);

        // ДО
        Console.WriteLine("Начальное состояние:");
        foreach (var customer in customerStorage.GetCustomers())
        {
            Console.WriteLine(customer);
        }
        Console.WriteLine();

        // Продаём машины
        hseCarService.SellCars();

        // ПОСЛЕ
        Console.WriteLine("Состояние после продажи:");
        foreach (var customer in customerStorage.GetCustomers())
        {
            Console.WriteLine(customer);
        }
    }
}