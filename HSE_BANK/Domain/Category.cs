namespace HSE_BANK.Domain;

public enum CategoryType
{
    Income,
    Expense
}

public class Category
{
    public Guid Id { get; private set; }
    public CategoryType Type { get; private set; }
    public string Name { get; private set; }

    public Category(CategoryType type, string name)
    {
        Id = Guid.NewGuid();
        Type = type;
        Name = name;
    }
}