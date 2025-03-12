using HSE_BANK.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HSE_BANK.DataAccess;

public class OperationDataAccess : IRepository<Operation>
{
    private readonly List<Operation> _operations = new List<Operation>();

    public void Add(Operation entity)
    {
        _operations.Add(entity);
    }

    public void Delete(Guid id)
    {
        var op = GetById(id);
        if (op != null)
        {
            _operations.Remove(op);
        }
    }

    public IEnumerable<Operation> GetAll()
    {
        return _operations;
    }

    public Operation GetById(Guid id)
    {
        return _operations.FirstOrDefault(o => o.Id == id);
    }

    public void Update(Operation entity)
    {
    }
}