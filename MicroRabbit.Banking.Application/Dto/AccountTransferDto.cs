namespace MicroRabbit.Banking.Application.Dto;

public class AccountTransferDto
{
    public int FromAccount { get; set; }
    public int ToAccount { get; set; }
    public decimal Amount { get; set; }
}