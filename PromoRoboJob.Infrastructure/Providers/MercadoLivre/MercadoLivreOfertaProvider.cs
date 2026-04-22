using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Infrastructure.Providers.MercadoLivre
{
    public class MercadoLivreOfertaProvider : IOfertaProvider
    {
        private readonly HttpClient _httpClient;

        public MercadoLivreOfertaProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<Oferta>> BuscarOfertasAsync(CategoriaMonitorada categoria, CancellationToken ct)
        {
            await Task.CompletedTask;

            return new List<Oferta>();
        }
    }
}
