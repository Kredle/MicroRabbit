using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MicroRabbit.Transfer.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();
        
        DependencyContainer.RegisterServices(builder.Services);

        builder.Services.AddDbContext<TransferDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TransferDbConnection"))
        );
        
        // Repositories
        builder.Services.AddScoped<ITransferRepository, TransferRepository>();
        
        // Services
        builder.Services.AddScoped<ITransferService, TransferService>();
        
        // Events
        builder.Services.AddScoped<IEventHandler<TransferCreatedEvent>, TransferCreatedEventHandler>();
        
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfer API", Version = "v1" });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transfer Microservice V1");
            });   
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();
        
        ConfigureEventBus(app);
        
        app.Run();
    }

    private static void ConfigureEventBus(IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        eventBus.Subscribe<TransferCreatedEvent, TransferCreatedEventHandler>();
    }
}