namespace MicroRabbit.Transfer.Domain.Models;

public class TransferLog
{
    public int TransferId { get; set; }
    public int FromAccount { get; set; }
    public int ToAccount { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public  TransferLog()
    {
        CreatedAt = DateTime.Now;
    }
}