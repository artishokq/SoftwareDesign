using HSE_BANK.Decorators;
using HSE_BANK.Commands;
using Xunit;

namespace Tests;

public class CommandTimerDecoratorTests
{
    private class TestCommand : ICommand
    {
        public bool Executed { get; private set; } = false;

        public void Execute()
        {
            Executed = true;
        }
    }

    [Fact]
    public void CommandTimerDecorator_ExecutesInnerCommand()
    {
        // Arrange
        var testCommand = new TestCommand();
        var decoratedCommand = new CommandTimerDecorator(testCommand);

        // Act
        decoratedCommand.Execute();

        // Assert
        Assert.True(testCommand.Executed);
    }
}