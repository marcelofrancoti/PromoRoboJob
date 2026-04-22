using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Infrastructure.Providers.Test
{
    public class TestOfertaProvider : IOfertaProvider
    {
        public Task<IReadOnlyCollection<Oferta>> BuscarOfertasAsync(CategoriaMonitorada categoria, CancellationToken ct)
        {
            var oferta = new Oferta
            {
                Id = Guid.NewGuid(),
                Titulo = "TV Smart 40\" Philco - Oferta Teste",
                UrlOriginal = "https://www.mercadolivre.com.br/teste/produto",
                UrlAfiliado = "https://www.mercadolivre.com.br/teste/produto?aff=meu",
                ImagemUrl = "https://http2.mlstatic.com/DUMMYIMAGE.jpg",
                PrecoOriginal = 2099.00m,
                PrecoAtual = 1259.00m,
                PercentualDesconto = Math.Round((2099.00m - 1259.00m) / 2099.00m * 100, 0),
                Categoria = categoria?.Nome ?? string.Empty,
                Plataforma = "MercadoLivre",
                Seller = "Loja Teste",
                DataColetaUtc = DateTime.UtcNow
            };

            return Task.FromResult((IReadOnlyCollection<Oferta>)new List<Oferta> { oferta });
        }
    }
}
