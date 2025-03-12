using HSE_BANK.DataAccess;
using HSE_BANK.Domain;
using Xunit;

namespace Tests;

public class DataAccessTests
{
    [Fact]
    public void BankAccountDataAccess_AddGetUpdateDelete_Works()
    {
        var dataAccess = new BankAccountDataAccess();
        var account = DomainFactory.CreateBankAccount("Test Account", 500);
        dataAccess.Add(account);

        var retrieved = dataAccess.GetById(account.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Test Account", retrieved.Name);

        // Обновление: изменение названия счета
        account.Name = "Updated Account";
        dataAccess.Update(account);
        retrieved = dataAccess.GetById(account.Id);
        Assert.Equal("Updated Account", retrieved.Name);

        dataAccess.Delete(account.Id);
        retrieved = dataAccess.GetById(account.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public void CategoryDataAccess_AddGetDelete_Works()
    {
        var dataAccess = new CategoryDataAccess();
        var category = new Category(CategoryType.Expense, "Food");
        dataAccess.Add(category);

        var retrieved = dataAccess.GetById(category.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Food", retrieved.Name);

        dataAccess.Delete(category.Id);
        retrieved = dataAccess.GetById(category.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public void OperationDataAccess_AddGetDelete_Works()
    {
        var dataAccess = new OperationDataAccess();
        var op = DomainFactory.CreateOperation(OperationType.Expense, Guid.NewGuid(), 100, DateTime.Now, Guid.NewGuid(),
            "Test Operation");
        dataAccess.Add(op);

        var retrieved = dataAccess.GetById(op.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(100, retrieved.Amount);

        dataAccess.Delete(op.Id);
        retrieved = dataAccess.GetById(op.Id);
        Assert.Null(retrieved);
    }
}