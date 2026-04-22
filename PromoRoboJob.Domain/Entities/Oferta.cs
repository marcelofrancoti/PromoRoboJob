using System;

namespace PromoRoboJob.Domain.Entities
{
    public class Oferta
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string UrlOriginal { get; set; } = string.Empty;
        public string UrlAfiliado { get; set; } = string.Empty;
        public string UrlCurta { get; set; } = string.Empty;
        public string ImagemUrl { get; set; } = string.Empty;
        public decimal PrecoAtual { get; set; }
        public decimal PrecoOriginal { get; set; }
        public decimal PercentualDesconto { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Plataforma { get; set; } = string.Empty;
        public string Seller { get; set; } = string.Empty;
        public bool JaEnviada { get; set; }
        public DateTime DataColetaUtc { get; set; }
    }
}
