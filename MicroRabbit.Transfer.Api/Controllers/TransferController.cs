using MicroRabbit.Transfer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferController : ControllerBase
{
    private readonly ITransferService _transferService;
    
    public TransferController(ITransferService transferService)
    {
        _transferService = transferService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTransferLogs()
    {
        var logs = await _transferService.GetTransferLogsAsync();
        return Ok(logs);
    }
}