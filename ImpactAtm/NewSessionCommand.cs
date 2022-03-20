namespace ImpactAtm;

public class NewSessionCommand : ICommand
{
    private readonly IAccountSession _session;

    public NewSessionCommand(IAccountSession session)
    {
        _session = session;
    }

    public object Execute(string[] args)
    {
        if (args.Length < 3)
            throw new ArgumentException("Too few arguments");

        var accountNumber = args[0];
        var expectedPin = args[1];
        var actualPin = args[2];
        //TODO: Should this throw an error if the previous session has not been ended?
        _session.IsValidated = false;
        _session.CurrentBalance = 0;
        //TODO: Check account number exists.
        if (expectedPin != actualPin)
            return ErrorStrings.AccountError;
        _session.AccountNumber = accountNumber;
        _session.IsValidated = true;
        return null;

    }
}