using System;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PromoRoboJob.Application.UseCases;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Worker.Worker
{
    public class OfertaWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OfertaWorker> _logger;
        private readonly IReadOnlyCollection<CategoriaMonitorada> _categorias;
        private readonly IConfiguration _configuration;

        public OfertaWorker(IServiceProvider serviceProvider, ILogger<OfertaWorker> logger, IReadOnlyCollection<CategoriaMonitorada> categorias, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _categorias = categorias ?? Array.Empty<CategoriaMonitorada>();
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OfertaWorker started");

            var intervaloConfig = _configuration.GetValue<int?>("Worker:IntervalMinutes") ?? 5;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var useCase = scope.ServiceProvider.GetRequiredService<BuscarEEnviarPromocoesUseCase>();

                    // Use configured categories (from appsettings or environment)
                    foreach (var categoria in _categorias)
                    {
                        if (!categoria.Ativa)
                            continue;

                        try
                        {
                            await useCase.ExecutarAsync(categoria, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Erro ao processar categoria {Categoria}", categoria.Nome);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro no worker");
                }

                await Task.Delay(TimeSpan.FromMinutes(intervaloConfig), stoppingToken);
            }

            _logger.LogInformation("OfertaWorker stopping");
        }
    }
}
