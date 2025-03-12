using System;
using System.Linq;
using HSE_BANK.DataAccess;
using HSE_BANK.Domain;
using HSE_BANK.Facades;
using Xunit;

namespace Tests;

public class AnalyticsTests
{
    [Fact]
    public void GetNetIncome_ReturnsCorrectDifference()
    {
        // Arrange
        var operationRepo = new OperationDataAccess();
        var categoryRepo = new CategoryDataAccess();
        var analyticsFacade = new AnalyticsFacade(operationRepo, categoryRepo);

        var accountId = Guid.NewGuid();
        var incomeCategory = DomainFactory.CreateCategory(CategoryType.Income, "Salary");
        var expenseCategory = DomainFactory.CreateCategory(CategoryType.Expense, "Food");
        categoryRepo.Add(incomeCategory);
        categoryRepo.Add(expenseCategory);

        var now = DateTime.Now;
        // Добавляем доход 1000
        var op1 = DomainFactory.CreateOperation(OperationType.Income, accountId, 1000, now, incomeCategory.Id);
        // Добавляем расход 300
        var op2 = DomainFactory.CreateOperation(OperationType.Expense, accountId, 300, now, expenseCategory.Id);
        operationRepo.Add(op1);
        operationRepo.Add(op2);

        // Act
        var netIncome = analyticsFacade.GetNetIncome(now.AddDays(-1), now.AddDays(1));

        // Assert
        Assert.Equal(700, netIncome);
    }

    [Fact]
    public void GetSumByCategory_ReturnsGroupedSum()
    {
        // Arrange
        var operationRepo = new OperationDataAccess();
        var categoryRepo = new CategoryDataAccess();
        var analyticsFacade = new AnalyticsFacade(operationRepo, categoryRepo);

        var accountId = Guid.NewGuid();
        var incomeCategory = DomainFactory.CreateCategory(CategoryType.Income, "Salary");
        categoryRepo.Add(incomeCategory);

        var now = DateTime.Now;
        // Добавляем две операции дохода
        var op1 = DomainFactory.CreateOperation(OperationType.Income, accountId, 1000, now, incomeCategory.Id);
        var op2 = DomainFactory.CreateOperation(OperationType.Income, accountId, 500, now, incomeCategory.Id);
        operationRepo.Add(op1);
        operationRepo.Add(op2);

        // Act
        var sumByCategory = analyticsFacade.GetSumByCategory(CategoryType.Income);

        // Assert
        Assert.True(sumByCategory.ContainsKey("Salary"));
        Assert.Equal(1500, sumByCategory["Salary"]);
    }
}