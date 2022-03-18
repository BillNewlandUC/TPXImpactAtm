namespace ImpactAtm;

public class AtmStartCommand : ICommand
{

    private readonly IAtm _atm;

    public AtmStartCommand(IAtm atm)
    {
        _atm = atm;
    }

    public object Execute(string[] args)
    {
        if (args.Length == 0)
            throw new ArgumentException("Too few arguments");
        var balance = decimal.Parse(args[0]);
        _atm.Balance = balance;
        return null;
    }
}