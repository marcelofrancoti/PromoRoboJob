using Microsoft.Extensions.DependencyInjection;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Application.UseCases;
using PromoRoboJob.Application.Validators;

namespace PromoRoboJob.Application.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<BuscarEEnviarPromocoesUseCase>();
            services.AddSingleton<ValidadorPromocao>();

            return services;
        }
    }
}
