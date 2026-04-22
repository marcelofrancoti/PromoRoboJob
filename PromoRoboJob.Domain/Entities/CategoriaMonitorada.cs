using System;

namespace PromoRoboJob.Domain.Entities
{
    public class CategoriaMonitorada
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string UrlOuTermoBusca { get; set; } = string.Empty;
        public decimal DescontoMinimo { get; set; }
        public bool Ativa { get; set; }
        public int IntervaloMinutos { get; set; }
    }
}
