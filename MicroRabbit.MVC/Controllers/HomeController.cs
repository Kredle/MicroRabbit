using System.Diagnostics;
using MicroRabbit.MVC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MicroRabbit.MVC.Models;
using MicroRabbit.MVC.Models.DTO;

namespace MicroRabbit.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITransferService _transferService;

    public HomeController(ILogger<HomeController> logger, ITransferService transferService)
    {
        _logger = logger;
        _transferService = transferService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> Transfer(TransferViewModel model)
    {
        var dto = new TransferDto
        {
            FromAccount = model.FromAccount,
            ToAccount = model.ToAccount,
            Amount = model.Amount
        };
        
        await _transferService.TransferAsync(dto);
        
        return RedirectToAction("Index");
    }
}