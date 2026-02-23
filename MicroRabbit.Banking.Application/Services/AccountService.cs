using MicroRabbit.Banking.Application.Dto;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEventBus _eventBus;
    
    public  AccountService(IAccountRepository accountRepository, IEventBus eventBus)
    {
        _accountRepository = accountRepository;
        _eventBus = eventBus;
    }

    public async Task<IEnumerable<Account>> GetAccounts()
    {
        return await _accountRepository.GetAccounts();
    }

    public async Task Transfer(AccountTransferDto transferDto)
    {
        var createTransferCommand = new Domain.Commands.CreateTransferCommand(
            transferDto.FromAccount,
            transferDto.ToAccount,
            transferDto.Amount
        );
        
        await _eventBus.SendCommand(createTransferCommand);
    }
}