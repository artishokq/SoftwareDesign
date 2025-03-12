using HSE_BANK.DataAccess;
using HSE_BANK.Domain;
using HSE_BANK.Facades;
using Xunit;

namespace Tests;

public class FacadesTests
{
    private BankAccountDataAccess CreateBankAccountDataAccess() => new BankAccountDataAccess();
    private CategoryDataAccess CreateCategoryDataAccess() => new CategoryDataAccess();
    private OperationDataAccess CreateOperationDataAccess() => new OperationDataAccess();

    [Fact]
    public void BankAccountFacade_CreateEditDeleteAccount_Works()
    {
        var bankAccountRepo = CreateBankAccountDataAccess();
        var facade = new BankAccountFacade(bankAccountRepo);

        // Создание счета
        var account = facade.CreateAccount("Test Account", 1000);
        Assert.NotNull(account);
        Assert.Equal("Test Account", account.Name);
        Assert.Equal(1000, account.Balance);

        // Изменение счета
        facade.EditAccount(account.Id, "Updated Account");
        var updatedAccount = bankAccountRepo.GetById(account.Id);
        Assert.Equal("Updated Account", updatedAccount.Name);

        // Удаление счета
        facade.DeleteAccount(account.Id);
        Assert.Null(bankAccountRepo.GetById(account.Id));
    }

    [Fact]
    public void CategoryFacade_CreateEditDeleteCategory_Works()
    {
        var categoryRepo = CreateCategoryDataAccess();
        var facade = new CategoryFacade(categoryRepo);

        // Создание категории
        var category = facade.CreateCategory(CategoryType.Income, "Salary");
        Assert.NotNull(category);
        Assert.Equal(CategoryType.Income, category.Type);
        Assert.Equal("Salary", category.Name);

        // Изменение категории
        facade.EditCategory(category.Id, "Bonus");
        var updatedCategory = categoryRepo.GetAll().FirstOrDefault(c => c.Name == "Bonus");
        Assert.NotNull(updatedCategory);
        Assert.Equal(CategoryType.Income, updatedCategory.Type);

        // Удаление категории
        facade.DeleteCategory(updatedCategory.Id);
        Assert.Null(categoryRepo.GetById(updatedCategory.Id));
    }

    [Fact]
    public void OperationFacade_CreateDeleteOperation_UpdatesBalance()
    {
        // Настройка репозиториев
        var bankAccountRepo = CreateBankAccountDataAccess();
        var operationRepo = CreateOperationDataAccess();

        var accountFacade = new BankAccountFacade(bankAccountRepo);
        var operationFacade = new OperationFacade(operationRepo, bankAccountRepo);

        // Создание счета и категории
        var account = accountFacade.CreateAccount("Test Account", 1000);
        var category = new Category(CategoryType.Income, "Salary");

        // Создание операции (доход)
        var op = operationFacade.CreateOperation(OperationType.Income, account.Id, 500, DateTime.Now, category.Id,
            "Salary payment");
        Assert.NotNull(op);
        // Проверка, что баланс увеличился на 500
        var updatedAccount = bankAccountRepo.GetById(account.Id);
        Assert.Equal(1500, updatedAccount.Balance);

        // Удаление операции, баланс должен вернуться к исходному значению
        operationFacade.DeleteOperation(op.Id);
        updatedAccount = bankAccountRepo.GetById(account.Id);
        Assert.Equal(1000, updatedAccount.Balance);
    }

    [Fact]
    public void AnalyticsFacade_GetNetIncome_Works()
    {
        var operationRepo = CreateOperationDataAccess();
        var categoryRepo = CreateCategoryDataAccess();
        var analyticsFacade = new AnalyticsFacade(operationRepo, categoryRepo);

        var now = DateTime.Now;
        var catIncome = new Category(CategoryType.Income, "Salary");
        var catExpense = new Category(CategoryType.Expense, "Food");
        categoryRepo.Add(catIncome);
        categoryRepo.Add(catExpense);

        // Добавление операций
        operationRepo.Add(DomainFactory.CreateOperation(OperationType.Income, Guid.NewGuid(), 1000, now, catIncome.Id));
        operationRepo.Add(DomainFactory.CreateOperation(OperationType.Expense, Guid.NewGuid(), 300, now,
            catExpense.Id));

        var netIncome = analyticsFacade.GetNetIncome(now.AddDays(-1), now.AddDays(1));
        Assert.Equal(700, netIncome);
    }
}