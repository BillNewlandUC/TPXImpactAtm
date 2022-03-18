namespace ImpactAtm;

public interface ICommandHandler
{
    object ParseAndExecute(string commandLine);
}