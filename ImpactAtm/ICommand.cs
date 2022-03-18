namespace ImpactAtm;

public interface ICommand
{
    public object Execute(string[] args);
}