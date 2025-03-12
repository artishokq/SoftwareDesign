namespace HSE_BANK.Domain;

public static class DomainFactory
{
    public static BankAccount CreateBankAccount(string name, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название счета не может быть пустым.");
        }

        return new BankAccount(name, initialBalance);
    }

    public static Category CreateCategory(CategoryType type, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название категории не может быть пустым.");
        }

        return new Category(type, name);
    }

    public static Operation CreateOperation(OperationType type, Guid bankAccountId, decimal amount, DateTime date,
        Guid categoryId, string description = null)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сумма операции должна быть положительной.");
        }

        return new Operation(type, bankAccountId, amount, date, categoryId, description);
    }
}