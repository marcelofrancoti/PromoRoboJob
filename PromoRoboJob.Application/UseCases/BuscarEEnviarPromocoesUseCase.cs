using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Application.UseCases
{
    public class BuscarEEnviarPromocoesUseCase
    {
        private readonly IEnumerable<IOfertaProvider> _providers;
        private readonly IEnumerable<INotificador> _notificadores;
        private readonly IGeradorMensagemPromocional _geradorMensagem;
        private readonly IEncurtadorUrl _encurtadorUrl;
        private readonly IOfertaRepository _ofertaRepository;
        private readonly Microsoft.Extensions.Logging.ILogger<BuscarEEnviarPromocoesUseCase> _logger;

        public BuscarEEnviarPromocoesUseCase(
            IEnumerable<IOfertaProvider> providers,
            IEnumerable<INotificador> notificadores,
            IGeradorMensagemPromocional geradorMensagem,
            IEncurtadorUrl encurtadorUrl,
            IOfertaRepository ofertaRepository,
            Microsoft.Extensions.Logging.ILogger<BuscarEEnviarPromocoesUseCase> logger)
        {
            _providers = providers;
            _notificadores = notificadores;
            _geradorMensagem = geradorMensagem;
            _encurtadorUrl = encurtadorUrl;
            _ofertaRepository = ofertaRepository;
            _logger = logger;
        }

        public async Task ExecutarAsync(CategoriaMonitorada categoria, CancellationToken ct)
        {
            foreach (var provider in _providers)
            {
                var ofertas = await provider.BuscarOfertasAsync(categoria, ct);

                foreach (var oferta in ofertas ?? Enumerable.Empty<Oferta>())
                {
                    if (oferta.PercentualDesconto < categoria.DescontoMinimo)
                        continue;

                    var jaFoiEnviada = await _ofertaRepository.JaFoiEnviadaAsync(oferta, ct);
                    if (jaFoiEnviada)
                        continue;

                    oferta.UrlCurta = await _encurtadorUrl.EncurtarAsync(oferta.UrlAfiliado, ct);
                    var mensagem = await _geradorMensagem.GerarAsync(oferta, ct);

                    foreach (var notificador in _notificadores)
                    {
                        try
                        {
                            await notificador.EnviarAsync(oferta, mensagem, ct);
                        }
                        catch (System.Exception ex)
                        {
                            // Log and continue with other notifiers/providers
                            _logger.LogError(ex, "Erro ao enviar oferta {OfertaTitulo} via {Notificador}", oferta.Titulo, notificador.GetType().Name);
                        }
                    }

                    await _ofertaRepository.MarcarComoEnviadaAsync(oferta, ct);
                }
            }
        }
    }
}
