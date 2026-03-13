using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Application.Interfaces;

public interface ITransferService
{
    public Task<IEnumerable<TransferLog>> GetTransferLogsAsync();
}