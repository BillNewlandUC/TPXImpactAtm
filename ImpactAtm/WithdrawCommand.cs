namespace ImpactAtm;

public class WithdrawCommand : ICommand
{
    private readonly IAccountSession _session;
    private readonly IAtm _atm;

    public WithdrawCommand(IAccountSession session, IAtm atm)
    {
        _session = session;
        _atm = atm;
    }

    public object Execute(string[] args)
    {
        if (args.Length < 1)
            throw new ArgumentException("Too few arguments");

        if (!_session.IsValidated)
            return ErrorStrings.AccountError;

        var amount = decimal.Parse(args[0]);

        if (_session.CurrentBalance + _session.OverdraftLimit < amount)
            return ErrorStrings.InsufficientFunds;

        if (_atm.Balance < amount)
            return ErrorStrings.AtmError;

        _session.CurrentBalance -= amount;
        _atm.Balance -= amount;
        return _session.CurrentBalance;
    }
}
