namespace ImpactAtm;

public class UnknownCommandException : Exception
{

    public string CommandName { get; private set; }

    public UnknownCommandException(string commandName = "",  string message = "")
        : base(message)
    {
        CommandName = commandName;
    }

}