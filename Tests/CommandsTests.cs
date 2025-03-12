using HSE_BANK.Commands;
using HSE_BANK.DataAccess;
using HSE_BANK.Domain;
using HSE_BANK.Facades;
using Xunit;

namespace Tests;

public class CommandsTests
{
    [Fact]
    public void AddOperationCommand_Executes_CreatesOperation()
    {
        var bankAccountRepo = new BankAccountDataAccess();
        var operationRepo = new OperationDataAccess();

        var accountFacade = new BankAccountFacade(bankAccountRepo);
        var operationFacade = new OperationFacade(operationRepo, bankAccountRepo);

        // Создание счета
        var account = accountFacade.CreateAccount("Test Account", 1000);
        // Создаем тестовую категорию
        var category = new Category(CategoryType.Income, "Salary");

        // Создаем команду для добавления операции
        var command = new AddOperationCommand(operationFacade, OperationType.Income, account.Id, 200, DateTime.Now,
            category.Id, "Test Command");
        command.Execute();

        // Проверка: операция создана и баланс счета обновлён
        var op = operationRepo.GetAll().FirstOrDefault();
        Assert.NotNull(op);
        Assert.Equal(200, op.Amount);

        var updatedAccount = bankAccountRepo.GetById(account.Id);
        Assert.Equal(1200, updatedAccount.Balance);
    }
}