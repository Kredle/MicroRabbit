using MediatR;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfer.Domain.EventHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(CreateTransferCommandHandler).Assembly,
            typeof(TransferCreatedEventHandler).Assembly,
            typeof(DependencyContainer).Assembly
        ));
        
        services.AddSingleton<IEventBus, RabbitMQBus>(sp => {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new RabbitMQBus(scopeFactory);
        });
    }
}