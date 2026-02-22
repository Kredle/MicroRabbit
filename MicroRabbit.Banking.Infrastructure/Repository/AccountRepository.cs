using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Banking.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Infrastructure.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly BankingDbContext _dbContext;
    
    public AccountRepository(BankingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IEnumerable<Account>> GetAccounts() =>
        _dbContext.Accounts.AsNoTracking().ToListAsync().ContinueWith(t => (IEnumerable<Account>)t.Result);
}