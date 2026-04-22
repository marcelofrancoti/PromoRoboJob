using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Application.Validators
{
    public class ValidadorPromocao
    {
        public bool EhValida(Oferta oferta, decimal descontoMinimo)
        {
            if (oferta == null) return false;

            if (string.IsNullOrWhiteSpace(oferta.Titulo))
                return false;

            if (oferta.PrecoOriginal <= 0 || oferta.PrecoAtual <= 0)
                return false;

            if (oferta.PrecoAtual >= oferta.PrecoOriginal)
                return false;

            if (oferta.PercentualDesconto < descontoMinimo)
                return false;

            if (string.IsNullOrWhiteSpace(oferta.ImagemUrl))
                return false;

            return true;
        }
    }
}
