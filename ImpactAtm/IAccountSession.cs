namespace ImpactAtm;

public interface IAccountSession
{
    
     decimal CurrentBalance { get; set; }

     bool IsValidated { get; set; }

     decimal OverdraftLimit { get; set; }

     string AccountNumber { get; set; }
}