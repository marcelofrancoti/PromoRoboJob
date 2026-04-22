using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Infrastructure.Encurtadores;
using PromoRoboJob.Infrastructure.Notificadores;
using PromoRoboJob.Infrastructure.Providers.MercadoLivre;
using PromoRoboJob.Infrastructure.Repositories.InMemory;
using PromoRoboJob.Infrastructure.Providers.Test;
using PromoRoboJob.Infrastructure.Geradores;

namespace PromoRoboJob.Infrastructure.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<MercadoLivreOfertaProvider>();
            services.AddHttpClient<TestOfertaProvider>();
            services.AddHttpClient<TinyUrlEncurtador>();
            // Register WhatsApp notifier HTTP client
            services.AddHttpClient<WhatsAppNotificador>();

            // Do not register Telegram notifier for test runs to avoid 404; only WhatsApp in test

            services.AddSingleton<IEncurtadorUrl, TinyUrlEncurtador>();
            services.AddSingleton<IOfertaRepository, OfertaRepository>();

            services.AddSingleton<IGeradorMensagemPromocional, PromoRoboJob.Infrastructure.Geradores.GeradorMensagemPromocional>();

            // For now always use Test provider and register WhatsApp notifier only (simpler for local testing)
            services.AddSingleton<IOfertaProvider, TestOfertaProvider>();

            // Register WhatsApp notifier as the only INotificador
            services.AddSingleton<INotificador>(sp => sp.GetRequiredService<WhatsAppNotificador>());

            return services;
        }
    }
}
