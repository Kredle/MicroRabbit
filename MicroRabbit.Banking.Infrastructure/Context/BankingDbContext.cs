using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Infrastructure.Context;

public class BankingDbContext : DbContext
{
    public  BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) {}
    
    // Models registration
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .ToTable("Accounts")
            .HasKey(a => a.Id);
        
        modelBuilder.Entity<Account>()
            .Property(a => a.Type)
            .IsRequired();
        
        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .IsRequired();
        
        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasPrecision(18, 4);
    }
}