using HSE_BANK.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HSE_BANK.DataAccess;

public class BankAccountDataAccess : IRepository<BankAccount>
{
    private readonly List<BankAccount> _accounts = new List<BankAccount>();

    public void Add(BankAccount entity)
    {
        _accounts.Add(entity);
    }

    public void Delete(Guid id)
    {
        var account = GetById(id);
        if (account != null)
        {
            _accounts.Remove(account);
        }
    }

    public IEnumerable<BankAccount> GetAll()
    {
        return _accounts;
    }

    public BankAccount GetById(Guid id)
    {
        return _accounts.FirstOrDefault(a => a.Id == id);
    }

    public void Update(BankAccount entity)
    {
        var account = GetById(entity.Id);
        if (account != null)
        {
            account.Name = entity.Name;
            // Изменение баланса происходит через методы Deposit/Withdraw
        }
    }
}