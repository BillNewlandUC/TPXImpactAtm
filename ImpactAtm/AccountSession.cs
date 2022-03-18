namespace ImpactAtm;

public class AccountSession : IAccountSession
{

    public decimal CurrentBalance { get; set; }
    
    public bool IsValidated { get;  set; }

    public decimal OverdraftLimit { get; set; }

    public string AccountNumber { get; set; }

}