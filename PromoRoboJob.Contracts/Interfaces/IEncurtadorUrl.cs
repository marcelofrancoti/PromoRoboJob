using System.Threading;
using System.Threading.Tasks;

namespace PromoRoboJob.Contracts.Interfaces
{
    public interface IEncurtadorUrl
    {
        Task<string> EncurtarAsync(string url, CancellationToken ct);
    }
}
