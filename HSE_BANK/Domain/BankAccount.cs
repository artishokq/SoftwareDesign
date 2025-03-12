namespace HSE_BANK.Domain;

public class BankAccount
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public decimal Balance { get; private set; }

    public BankAccount(string name, decimal initialBalance = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сумма пополнения должна быть положительной.");
        }

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сумма снятия должна быть положительной.");
        }

        if (Balance < amount)
        {
            throw new InvalidOperationException("Недостаточно средств.");
        }

        Balance -= amount;
    }
}