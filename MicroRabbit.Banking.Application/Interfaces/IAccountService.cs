using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Application.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<Account>> GetAccounts();
}