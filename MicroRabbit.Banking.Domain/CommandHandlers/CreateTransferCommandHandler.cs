using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain.CommandHandlers;

public class CreateTransferCommandHandler: IRequestHandler<CreateTransferCommand, bool>
{
    private readonly IEventBus _eventBus;
    
    public CreateTransferCommandHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public async Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
    {
        // Publish event to RabbitMQ
        await _eventBus.Publish(new TransferCreatedEvent(request.FromAccount, request.ToAccount, request.Amount));
        
        return true;
    }
}