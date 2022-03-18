namespace ImpactAtm;

public interface ICommandResolver
{
    ICommand GetCommand(string commandName);
}