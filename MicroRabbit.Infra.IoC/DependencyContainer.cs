using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Infrastructure.Repository;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyContainer).Assembly));
        
        // Domain Bus
        services.AddTransient<IEventBus, RabbitMQBus>();
        
        // Application Services
        services.AddScoped<IAccountService, AccountService>();
        
        // Data
        services.AddScoped<IAccountRepository, AccountRepository>();
    }
}