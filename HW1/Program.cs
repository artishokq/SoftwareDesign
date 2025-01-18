namespace HW1;

internal class Program
{
    static void Main(string[] args)
    {
        var factory = new FactoryAF();
        
        factory.AddCar();
        factory.AddCar();
        factory.AddCar();
        factory.AddCar();
        factory.AddCar();
        
        factory.AddCustomer(new Customer("Иванов Иван Иванович"));
        factory.AddCustomer(new Customer("Петров Петр Петрович"));
        factory.AddCustomer(new Customer("Сидоров Сидор Сидорович"));
        factory.AddCustomer(new Customer("Смирнов Николай Николаевич"));

        Console.WriteLine("Before");
        Console.WriteLine(string.Join(Environment.NewLine, factory.Cars));
        Console.WriteLine(string.Join(Environment.NewLine, factory.Customers));

        factory.SaleCar();

        Console.WriteLine("After");
        Console.WriteLine(string.Join(Environment.NewLine, factory.Cars));
        Console.WriteLine(string.Join(Environment.NewLine, factory.Customers));
    }
}