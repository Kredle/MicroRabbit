using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;

namespace MicroRabbit.Transfer.Domain.EventHandlers;

public class TransferCreatedEventHandler: IEventHandler<TransferCreatedEvent>
{
    private readonly ITransferRepository _transferRepository;
    
    public TransferCreatedEventHandler(ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
    }
    
    public async Task Handle(TransferCreatedEvent @event)
    {
        Console.WriteLine($"[TransferCreatedEventHandler] Received event From={@event.FromAccount} To={@event.ToAccount} Amount={@event.Amount}");
        try
        {
            await _transferRepository.AddTransferLogAsync(
                @event.FromAccount,
                @event.ToAccount,
                @event.Amount
            ).ConfigureAwait(false);
            Console.WriteLine("[TransferCreatedEventHandler] AddTransferLogAsync completed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TransferCreatedEventHandler] ERROR adding transfer log: {ex}");
            throw;
        }
    }
}