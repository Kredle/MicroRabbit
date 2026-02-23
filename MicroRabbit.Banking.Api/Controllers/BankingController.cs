using MicroRabbit.Banking.Application.Dto;
using MicroRabbit.Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankingController : ControllerBase
{
    private readonly IAccountService _accountService;
    
    public BankingController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    // GET api/banking
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _accountService.GetAccounts());
    }
    
    // POST api/banking
    [HttpPost]
    public async Task<IActionResult> Transfer([FromBody] AccountTransferDto transferDto)
    {
        await _accountService.Transfer(transferDto);
        
        return Ok(transferDto);
    }
}