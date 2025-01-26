namespace HW2.Service;

using HW2.Car;
using HW2.Customer;
using HW2.Engine;

public class CustomerStorage : ICustomersProvider
{
    private readonly List<Customer> _customers = new();

    public void AddCustomer(Customer customer)
    {
        _customers.Add(customer);
    }

    public IEnumerable<Customer> GetCustomers()
    {
        return _customers;
    }
}