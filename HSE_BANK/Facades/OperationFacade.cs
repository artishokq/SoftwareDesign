using HSE_BANK.DataAccess;
using HSE_BANK.Domain;

namespace HSE_BANK.Facades;

public class OperationFacade
{
    private readonly IRepository<Operation> _operationRepository;
    private readonly IRepository<BankAccount> _accountRepository;

    public OperationFacade(IRepository<Operation> operationRepository, IRepository<BankAccount> accountRepository)
    {
        _operationRepository = operationRepository;
        _accountRepository = accountRepository;
    }

    public Operation CreateOperation(OperationType type, Guid bankAccountId, decimal amount, DateTime date,
        Guid categoryId, string description = null)
    {
        var operation = DomainFactory.CreateOperation(type, bankAccountId, amount, date, categoryId, description);
        _operationRepository.Add(operation);
        // Обновляем баланс счета
        var account = _accountRepository.GetById(bankAccountId);
        if (account != null)
        {
            if (type == OperationType.Income)
            {
                account.Deposit(amount);
            }
            else
            {
                account.Withdraw(amount);
            }

            _accountRepository.Update(account);
        }

        return operation;
    }

    public void DeleteOperation(Guid operationId)
    {
        var operation = _operationRepository.GetById(operationId);
        if (operation == null)
        {
            throw new Exception("Операция не найдена");
        }

        // Обратное действие для баланса
        var account = _accountRepository.GetById(operation.BankAccountId);
        if (account != null)
        {
            if (operation.Type == OperationType.Income)
            {
                account.Withdraw(operation.Amount);
            }
            else
            {
                account.Deposit(operation.Amount);
            }

            _accountRepository.Update(account);
        }

        _operationRepository.Delete(operationId);
    }
}