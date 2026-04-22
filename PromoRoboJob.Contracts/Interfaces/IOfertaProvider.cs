using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Contracts.Interfaces
{
    public interface IOfertaProvider
    {
        Task<IReadOnlyCollection<Oferta>> BuscarOfertasAsync(CategoriaMonitorada categoria, CancellationToken ct);
    }
}
