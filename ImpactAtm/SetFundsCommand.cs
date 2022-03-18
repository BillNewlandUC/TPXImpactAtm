namespace ImpactAtm;

public class SetFundsCommand : ICommand
{
    private readonly IAccountSession _session;

    public SetFundsCommand(IAccountSession session)
    {
        _session = session;
    }
    public object Execute( string[] args)
    {
        if (args.Length < 2)
            throw new ArgumentException("Too few arguments");

        if(!_session.IsValidated)
            return ErrorStrings.AccountError;

        var balance = decimal.Parse(args[0]);
        var overdraft = decimal.Parse(args[1]);
        _session.CurrentBalance = balance;
        _session.OverdraftLimit = overdraft;
        return null;
    }
}