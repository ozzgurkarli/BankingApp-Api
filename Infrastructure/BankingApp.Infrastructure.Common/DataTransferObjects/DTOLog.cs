namespace BankingApp.Infrastructure.Common.DataTransferObjects;

public class DTOLog: BaseDTO    
{
    public string? Operation { get; set; }

    public bool APICall { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public string? ErrorMessage { get; set; }

    public string? CallerIP { get; set; }

    public string Caller { get; set; }
}