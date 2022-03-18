namespace ImpactAtm;

public class BalanceEnquiryCommand : ICommand
{
    private readonly IAccountSession _session;

    public BalanceEnquiryCommand(IAccountSession session)
    {
        _session = session;
    }

    public object Execute(string[] args)
    {
        if (!_session.IsValidated)
            return ErrorStrings.AccountError;
        return _session.CurrentBalance;

    }
}