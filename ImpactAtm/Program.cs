using ImpactAtm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//setup our DI
var serviceProvider = new ServiceCollection()
    .AddLogging(c => c.AddConsole())
    .AddSingleton<IAccountSession, AccountSession>()
    .AddSingleton<IAtm, Atm>()
    .AddTransient<ICommandHandler, CommandHandler>()
    .AddTransient<ICommandResolver, CommandResolver>()
    .AddTransient<AtmStartCommand>()
    .AddTransient<BalanceEnquiryCommand>()
    .AddTransient<EndSessionCommand>()
    .AddTransient<NewSessionCommand>()
    .AddTransient<SetFundsCommand>()
    .AddTransient<NewSessionCommand>()
    .AddTransient<WithdrawCommand>()
    .BuildServiceProvider();

var handler = serviceProvider.GetService<ICommandHandler>();
if (handler != null)
{
    Console.WriteLine("##########################################");
    Console.WriteLine("######        TPXImpact ATM         ######");
    Console.WriteLine("######          Welcome!            ######");
    Console.WriteLine("##########################################");
    Console.WriteLine();
    Console.WriteLine("Enter command:");
    while (true)
    {
        var line = Console.ReadLine();
        if (string.Compare(line, "q", StringComparison.CurrentCultureIgnoreCase) == 0)
            break;
        var result = handler.ParseAndExecute(line);
        if (result != null)
            Console.WriteLine(result);
    }
    Console.WriteLine("Goodbye!");
}


