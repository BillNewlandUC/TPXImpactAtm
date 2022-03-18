namespace ImpactAtm;

public class CommandResolver : ICommandResolver
{
    private readonly IServiceProvider _serviceProvider;

    public CommandResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICommand GetCommand(string commandName)
    {

        var command = (ICommand)(commandName.ToUpper() switch
        {
            "A" => _serviceProvider.GetService(typeof(AtmStartCommand)),
            "B" => _serviceProvider.GetService(typeof(BalanceEnquiryCommand)),
            "Z" => _serviceProvider.GetService(typeof(EndSessionCommand)),
            "N" => _serviceProvider.GetService(typeof(NewSessionCommand)),
            "F" => _serviceProvider.GetService(typeof(SetFundsCommand)),
            "W" => _serviceProvider.GetService(typeof(WithdrawCommand)),
            _ => throw new ArgumentOutOfRangeException(nameof(commandName), commandName, null)
        });
        if (command == null)
            throw new UnknownCommandException();
        return command;
    }

     
}