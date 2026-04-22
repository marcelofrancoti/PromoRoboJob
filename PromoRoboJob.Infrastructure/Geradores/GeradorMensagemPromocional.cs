using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Infrastructure.Geradores
{
    public class GeradorMensagemPromocional : IGeradorMensagemPromocional
    {
        public Task<string> GerarAsync(Oferta oferta, CancellationToken ct)
        {
            var msg = $"?? OFERTA ENCONTRADA\n\n{oferta.Titulo}\n\nDe: R$ {oferta.PrecoOriginal:N2} -> Por: R$ {oferta.PrecoAtual:N2} ({oferta.PercentualDesconto:N0}% OFF)\n\n{oferta.UrlAfiliado}";
            return Task.FromResult(msg);
        }
    }
}
