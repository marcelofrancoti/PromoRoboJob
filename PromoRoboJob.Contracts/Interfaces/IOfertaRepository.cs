using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Contracts.Interfaces
{
    public interface IOfertaRepository
    {
        Task<bool> JaFoiEnviadaAsync(Oferta oferta, CancellationToken ct);
        Task MarcarComoEnviadaAsync(Oferta oferta, CancellationToken ct);
    }
}
