using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Contracts.Interfaces
{
    public interface IGeradorMensagemPromocional
    {
        Task<string> GerarAsync(Oferta oferta, CancellationToken ct);
    }
}
