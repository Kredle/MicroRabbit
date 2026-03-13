using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Infrastructure.Context;

public class TransferDbContext: DbContext
{
    public TransferDbContext(DbContextOptions<TransferDbContext> options) : base(options)
    {
    }
    
    public DbSet<TransferLog> TransferLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransferLog>().ToTable("TransferLogs");
        
        modelBuilder.Entity<TransferLog>()
            .HasKey(x => x.TransferId);

        modelBuilder.Entity<TransferLog>()
            .Property(x => x.TransferId)
            .IsRequired();
        
        modelBuilder.Entity<TransferLog>()
            .Property(x => x.TransferId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<TransferLog>()
            .Property(x => x.FromAccount)
            .IsRequired();
        
        modelBuilder.Entity<TransferLog>()
            .Property(x => x.ToAccount)
            .IsRequired();
        
        modelBuilder.Entity<TransferLog>()
            .Property(x => x.Amount)
            .HasPrecision(18, 4);
        
        modelBuilder.Entity<TransferLog>()
            .Property(x => x.Amount)
            .IsRequired();

        modelBuilder.Entity<TransferLog>()
            .Property(x => x.CreatedAt)
            .HasColumnType("datetime2(0)");
    }
}