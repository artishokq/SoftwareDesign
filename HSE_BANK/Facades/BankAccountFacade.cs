using HSE_BANK.DataAccess;
using HSE_BANK.Domain;

namespace HSE_BANK.Facades;

public class BankAccountFacade
{
    private readonly IRepository<BankAccount> _accountRepository;

    public BankAccountFacade(IRepository<BankAccount> accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public BankAccount CreateAccount(string name, decimal initialBalance = 0)
    {
        var account = DomainFactory.CreateBankAccount(name, initialBalance);
        _accountRepository.Add(account);
        return account;
    }

    public void EditAccount(Guid id, string newName)
    {
        var account = _accountRepository.GetById(id);
        if (account == null)
            throw new Exception("Счет не найден");
        account.Name = newName;
        _accountRepository.Update(account);
    }

    public void DeleteAccount(Guid id)
    {
        _accountRepository.Delete(id);
    }
}