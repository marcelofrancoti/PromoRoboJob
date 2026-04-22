using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Contracts.Interfaces;

namespace PromoRoboJob.Infrastructure.Encurtadores
{
    public class TinyUrlEncurtador : IEncurtadorUrl
    {
        private readonly HttpClient _httpClient;

        public TinyUrlEncurtador(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> EncurtarAsync(string url, CancellationToken ct)
        {
            // stub: just return the original for now
            await Task.CompletedTask;
            return url;
        }
    }
}
