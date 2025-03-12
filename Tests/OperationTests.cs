using System;
using HSE_BANK.Domain;
using Xunit;

namespace Tests;

public class OperationTests
{
    [Fact]
    public void CreateOperation_WithNegativeAmount_ThrowsException()
    {
        // Arrange
        var bankAccountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            DomainFactory.CreateOperation(OperationType.Expense, bankAccountId, -10, DateTime.Now, categoryId));
    }

    [Fact]
    public void CreateOperation_WithValidAmount_CreatesOperation()
    {
        // Arrange
        var bankAccountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var operation =
            DomainFactory.CreateOperation(OperationType.Income, bankAccountId, 100, DateTime.Now, categoryId, "Test");

        // Assert
        Assert.Equal(100, operation.Amount);
        Assert.Equal("Test", operation.Description);
    }
}