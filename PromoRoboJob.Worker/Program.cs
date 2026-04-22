using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoRoboJob.Application.DI;
using PromoRoboJob.Domain.Entities;
using PromoRoboJob.Infrastructure.DI;
using PromoRoboJob.Worker.Worker;
using System.Collections.Generic;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddInfra(ctx.Configuration);
        services.AddApplication();

        // Bind categories from configuration (appsettings.json or environment)
        var categorias = ctx.Configuration.GetSection("Categorias").Get<List<CategoriaMonitorada>>() ?? new List<CategoriaMonitorada>();
        services.AddSingleton<IReadOnlyCollection<CategoriaMonitorada>>(categorias);

        services.AddHostedService<OfertaWorker>();
    })
    .Build();

await host.RunAsync();
