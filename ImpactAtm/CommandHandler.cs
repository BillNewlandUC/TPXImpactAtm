namespace ImpactAtm;

public class CommandHandler : ICommandHandler
{
    private readonly ICommandResolver _resolver;

    public CommandHandler(ICommandResolver resolver)
    {
        _resolver = resolver;
    }

    public object ParseAndExecute(string commandLine)
    {
        string commandName;
        var args = Array.Empty<string>();

        var parts = commandLine.Split(" ",StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            commandName = "Z"; // End session
        else
        {
            if (decimal.TryParse(parts[0], out _))
            {
                commandName = parts.Length switch
                {
                    1 => "A", // Set ATM funds
                    3 => "N", // new session
                    2 => "F", // Set account funds
                    _ => string.Empty
                };
                args = parts;
            }
            else
            {
                args = parts.Skip(1).ToArray();
                commandName = parts[0];
            }
        }

        if (string.IsNullOrEmpty(commandName))
        {
            //TODO: Log incorrect command name
            return null;
        }

        try
        {
            var command = _resolver.GetCommand(commandName);
            return command.Execute(args);
        }
        catch (ArgumentException)
        {
            //TODO: Log malformed commands)
            return ErrorStrings.CommandError;
        }
        catch (Exception)
        {
            //TODO: Log other errors
            return null;
        }
    }
}