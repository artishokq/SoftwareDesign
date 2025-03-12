using HSE_BANK.Commands;
using System.Diagnostics;

namespace HSE_BANK.Decorators;

public class CommandTimerDecorator : ICommand
{
    private readonly ICommand _innerCommand;

    public CommandTimerDecorator(ICommand innerCommand)
    {
        _innerCommand = innerCommand;
    }

    public void Execute()
    {
        var stopwatch = Stopwatch.StartNew();
        _innerCommand.Execute();
        stopwatch.Stop();
        Console.WriteLine($"Команда выполнена за {stopwatch.ElapsedMilliseconds} мс.");
    }
}