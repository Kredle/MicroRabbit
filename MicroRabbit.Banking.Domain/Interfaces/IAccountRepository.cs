using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Domain.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAccounts();
}