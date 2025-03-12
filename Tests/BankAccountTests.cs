using System;
using HSE_BANK.Domain;
using Xunit;

namespace Tests;

public class BankAccountTests
{
    [Fact]
    public void Deposit_PositiveAmount_IncreasesBalance()
    {
        // Arrange
        var account = DomainFactory.CreateBankAccount("Test Account", 100);

        // Act
        account.Deposit(50);

        // Assert
        Assert.Equal(150, account.Balance);
    }

    [Fact]
    public void Withdraw_PositiveAmount_DecreasesBalance()
    {
        // Arrange
        var account = DomainFactory.CreateBankAccount("Test Account", 100);

        // Act
        account.Withdraw(40);

        // Assert
        Assert.Equal(60, account.Balance);
    }

    [Fact]
    public void Withdraw_MoreThanBalance_ThrowsException()
    {
        // Arrange
        var account = DomainFactory.CreateBankAccount("Test Account", 100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => account.Withdraw(150));
    }
}