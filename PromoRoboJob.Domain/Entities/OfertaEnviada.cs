using System;

namespace PromoRoboJob.Domain.Entities
{
    public class OfertaEnviada
    {
        public Guid Id { get; set; }
        public string HashOferta { get; set; } = string.Empty;
        public string Canal { get; set; } = string.Empty;
        public DateTime DataEnvioUtc { get; set; }
    }
}
