using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using MicroRabbit.Transfer.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.IO;

public class TransferRepository : ITransferRepository
{
    private readonly TransferDbContext _context;

    public TransferRepository(TransferDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransferLog>> GetTransferLogsAsync()
    {
        return await _context.TransferLogs
            .AsNoTracking()
            .OrderByDescending(x => x.TransferId)
            .ToListAsync();
    }

    public async Task AddTransferLogAsync(int fromAccount, int toAccount, decimal amount)
    {
        var transferLog = new TransferLog
        {
            FromAccount = fromAccount,
            ToAccount = toAccount,
            Amount = amount
        };
        
        await _context.TransferLogs.AddAsync(transferLog);
        await _context.SaveChangesAsync();
    }
}