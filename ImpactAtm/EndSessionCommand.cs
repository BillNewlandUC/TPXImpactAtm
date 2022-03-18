namespace ImpactAtm;

public class EndSessionCommand : ICommand
{
    private readonly IAccountSession _session;

    public EndSessionCommand(IAccountSession session)
    {
        _session = session;
    }

    public object Execute( string[] args)
    {
        _session.IsValidated = false;
        _session.AccountNumber = null;
        _session.CurrentBalance = 0;
        _session.OverdraftLimit = 0;
        return null;
    }
}