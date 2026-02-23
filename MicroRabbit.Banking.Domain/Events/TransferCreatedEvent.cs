using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Banking.Domain.Events;

public class TransferCreatedEvent: Event
{
    public int FromAccount { get; private set; }
    public int ToAccount { get; private set; }
    public decimal Amount { get; private set; }
    
    public TransferCreatedEvent(int from, int to, decimal amount)
    {
        FromAccount = from;
        ToAccount = to;
        Amount = amount;
    }
}