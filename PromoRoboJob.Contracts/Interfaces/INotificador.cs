using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Contracts.Interfaces
{
    public interface INotificador
    {
        Task EnviarAsync(Oferta oferta, string mensagem, CancellationToken ct);
    }
}
