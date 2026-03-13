using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Infrastructure.Context;
using MicroRabbit.Banking.Infrastructure.Repository;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MicroRabbit.Banking.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        DependencyContainer.RegisterServices(builder.Services);

        builder.Services.AddDbContext<BankingDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BankingDbConnection"))
        );
        
        // Services
        builder.Services.AddScoped<IAccountService, AccountService>();
        
        // Repositories
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        
        // Commands
        builder.Services.AddScoped<IRequestHandler<CreateTransferCommand, bool>, CreateTransferCommandHandler>();
        
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Banking API", Version = "v1" });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Microservice V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Map controller endpoints
        app.MapControllers();

        app.Run();
    }
}