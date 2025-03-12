using HSE_BANK.DataAccess;
using HSE_BANK.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HSE_BANK.Facades;

public class AnalyticsFacade
{
    private readonly IRepository<Operation> _operationRepository;
    private readonly IRepository<Category> _categoryRepository;

    public AnalyticsFacade(IRepository<Operation> operationRepository, IRepository<Category> categoryRepository)
    {
        _operationRepository = operationRepository;
        _categoryRepository = categoryRepository;
    }

    // Разница между доходами и расходами за период
    public decimal GetNetIncome(DateTime startDate, DateTime endDate)
    {
        var operations = _operationRepository.GetAll()
            .Where(o => o.Date >= startDate && o.Date <= endDate);
        decimal income = operations.Where(o => o.Type == OperationType.Income).Sum(o => o.Amount);
        decimal expense = operations.Where(o => o.Type == OperationType.Expense).Sum(o => o.Amount);
        return income - expense;
    }

    // Группировка операций по категориям
    public Dictionary<string, decimal> GetSumByCategory(CategoryType categoryType)
    {
        var operations = _operationRepository.GetAll()
            .Where(o => (categoryType == CategoryType.Income && o.Type == OperationType.Income) ||
                        (categoryType == CategoryType.Expense && o.Type == OperationType.Expense));

        var result = new Dictionary<string, decimal>();
        foreach (var op in operations)
        {
            var category = _categoryRepository.GetById(op.CategoryId);
            if (category != null)
            {
                if (!result.ContainsKey(category.Name))
                    result[category.Name] = 0;
                result[category.Name] += op.Amount;
            }
        }

        return result;
    }
}