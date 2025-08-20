namespace BankingApp.Infrastructure.Common.DataTransferObjects;

public class DTOOperation: BaseDTO
{
    public string? ServiceName { get; set; }

    public string? OperationName { get; set; }

    public bool? Active { get; set; }
}