using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Infrastructure.Repositories.InMemory
{
    public class OfertaRepository : IOfertaRepository
    {
        private readonly ConcurrentDictionary<string, bool> _sent = new();

        public Task<bool> JaFoiEnviadaAsync(Oferta oferta, CancellationToken ct)
        {
            var key = ComputeKey(oferta);
            return Task.FromResult(_sent.ContainsKey(key));
        }

        public Task MarcarComoEnviadaAsync(Oferta oferta, CancellationToken ct)
        {
            var key = ComputeKey(oferta);
            _sent.TryAdd(key, true);
            return Task.CompletedTask;
        }

        private string ComputeKey(Oferta oferta)
        {
            return oferta.UrlAfiliado ?? oferta.UrlOriginal ?? oferta.Titulo;
        }
    }
}
