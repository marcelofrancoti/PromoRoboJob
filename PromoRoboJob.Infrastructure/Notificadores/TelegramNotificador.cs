using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Infrastructure.Notificadores
{
    public class TelegramNotificador : INotificador
    {
        private readonly HttpClient _httpClient;
        private readonly string _botToken;
        private readonly string _chatId;

        public TelegramNotificador(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _botToken = configuration["Telegram:BotToken"] ?? string.Empty;
            _chatId = configuration["Telegram:ChatId"] ?? string.Empty;
        }

        public async Task EnviarAsync(Oferta oferta, string mensagem, CancellationToken ct)
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendPhoto";

            var payload = new
            {
                chat_id = _chatId,
                photo = oferta.ImagemUrl,
                caption = mensagem,
                parse_mode = "HTML"
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload, ct);
            response.EnsureSuccessStatusCode();
        }
    }
}
