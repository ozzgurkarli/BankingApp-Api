using BankingApp.Infrastructure.Common.DataTransferObjects;

namespace BankingApp.Infrastructure.Common.DataTransferObjects;

public class DTOMail: BaseDTO
{
    public string? To { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }
}