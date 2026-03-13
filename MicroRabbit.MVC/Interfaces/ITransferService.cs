using MicroRabbit.MVC.Models.DTO;

namespace MicroRabbit.MVC.Interfaces;

public interface ITransferService
{
    Task TransferAsync(TransferDto dto);
}