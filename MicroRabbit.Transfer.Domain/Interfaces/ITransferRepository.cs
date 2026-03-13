using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.Interfaces;

public interface ITransferRepository
{
    public Task<IEnumerable<TransferLog>> GetTransferLogsAsync();
    public Task AddTransferLogAsync(int fromAccount, int toAccount, decimal amount);
}