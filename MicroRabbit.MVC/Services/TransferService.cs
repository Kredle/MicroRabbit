using System.Text;
using MicroRabbit.MVC.Interfaces;
using MicroRabbit.MVC.Models.DTO;
using Newtonsoft.Json;

namespace MicroRabbit.MVC.Services;

public class TransferService: ITransferService
{
    private readonly HttpClient _httpClient;
    
    public TransferService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task TransferAsync(TransferDto dto)
    {
        var uri = "api/Banking";
        var transferContent = new StringContent(JsonConvert.SerializeObject(dto),
            Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(uri, transferContent);
        response.EnsureSuccessStatusCode();
    }
}