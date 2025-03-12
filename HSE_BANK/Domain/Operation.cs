namespace HSE_BANK.Domain;

public enum OperationType
{
    Income,
    Expense
}

public class Operation
{
    public Guid Id { get; private set; }
    public OperationType Type { get; private set; }
    public Guid BankAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public Guid CategoryId { get; private set; }

    public Operation(OperationType type, Guid bankAccountId, decimal amount, DateTime date, Guid categoryId,
        string description = null)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Сумма операции не может быть отрицательной.");
        }

        Id = Guid.NewGuid();
        Type = type;
        BankAccountId = bankAccountId;
        Amount = amount;
        Date = date;
        CategoryId = categoryId;
        Description = description;
    }
}