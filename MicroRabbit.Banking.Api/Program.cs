using MicroRabbit.Banking.Infrastructure.Context;
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

        // Add controllers so MVC controllers are discovered (Swagger needs this to include operations)
        builder.Services.AddControllers();
        
        DependencyContainer.RegisterServices(builder.Services);

        builder.Services.AddDbContext<BankingDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BankingDbConnection"))
        );
        
        builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Banking API", Version = "v1" });
            });
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        
        // Ensure the Swagger JSON is available for the UI
        app.UseSwagger();
        
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Microservice V1");
        });

        app.UseAuthorization();

        // Map controller endpoints
        app.MapControllers();

        app.Run();
    }
}