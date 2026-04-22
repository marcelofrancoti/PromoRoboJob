using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PromoRoboJob.Contracts.Interfaces;
using PromoRoboJob.Domain.Entities;

namespace PromoRoboJob.Infrastructure.Notificadores
{
    public class WhatsAppNotificador : INotificador
    {
        private readonly HttpClient _httpClient;
        private readonly List<string> _recipients = new();
        private readonly string _instanceId = string.Empty;
        private readonly string _baseUrl = string.Empty;
        private readonly string _apiKey = string.Empty;
        private readonly string _provider = string.Empty;
        private readonly string _phoneNumberId = string.Empty;
        private readonly string _apiKeyHeader = "Authorization";
        private readonly string _apiKeyScheme = "Bearer";
        private readonly string _sendTextPath = "/message/sendText/{instanceId}";
        private readonly IDictionary<string, string> _additionalHeaders = new Dictionary<string, string>();
        private readonly ILogger<WhatsAppNotificador> _logger;
        public WhatsAppNotificador(HttpClient httpClient, IConfiguration configuration, ILogger<WhatsAppNotificador> logger)
        {
            _httpClient = httpClient;

            _logger = logger;

            _instanceId = configuration["WhatsApp:InstanceId"] ?? string.Empty;
            _baseUrl = configuration["WhatsApp:BaseUrl"] ?? string.Empty;
            _apiKey = configuration["WhatsApp:ApiKey"] ?? string.Empty;
            _provider = configuration["WhatsApp:Provider"] ?? string.Empty;
            _phoneNumberId = configuration["WhatsApp:PhoneNumberId"] ?? string.Empty;
            _apiKeyHeader = configuration["WhatsApp:ApiKeyHeader"] ?? _apiKeyHeader;
            _apiKeyScheme = configuration["WhatsApp:ApiKeyScheme"] ?? _apiKeyScheme;
            _sendTextPath = configuration["WhatsApp:SendTextPath"] ?? _sendTextPath;

            var recipients = configuration.GetSection("WhatsApp:Recipients").Get<List<string>>();
            if (recipients != null)
                _recipients.AddRange(recipients);

            // Additional headers optional
            var headersSection = configuration.GetSection("WhatsApp:Headers").GetChildren();
            foreach (var h in headersSection)
            {
                var key = h.Key;
                var val = h.Value;
                if (!string.IsNullOrWhiteSpace(key) && val != null)
                    _additionalHeaders[key] = val;
            }
        }
        public async Task EnviarAsync(Oferta oferta, string mensagem, CancellationToken ct)
        {
            if (_recipients.Count == 0)
                return;

            var path = _sendTextPath
                .Replace("{instanceId}", _instanceId ?? string.Empty)
                .Replace("{phoneNumberId}", _phoneNumberId ?? string.Empty);
            var endpoint = string.IsNullOrWhiteSpace(_baseUrl)
                ? path
                : $"{_baseUrl.TrimEnd('/')}/{path.TrimStart('/')}";

            foreach (var to in _recipients)
            {
                object payload;
                string payloadSerialized;

                if (string.Equals(_provider, "WhatsAppCloud", StringComparison.OrdinalIgnoreCase))
                {
                    // Meta / WhatsApp Cloud API payload
                    payload = new
                    {
                        messaging_product = "whatsapp",
                        to = to,
                        type = "text",
                        text = new { body = mensagem }
                    };
                    payloadSerialized = JsonSerializer.Serialize(payload);
                    _logger?.LogInformation("WhatsAppCloud: sending to {To} endpoint={Endpoint} payload={Payload}", to, endpoint, payloadSerialized);

                    using var cloudRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
                    {
                        Content = JsonContent.Create(payload)
                    };

                    if (!string.IsNullOrWhiteSpace(_apiKey))
                    {
                        // Always use Authorization: Bearer for WhatsApp Cloud
                        cloudRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                    }

                    foreach (var kv in _additionalHeaders)
                        cloudRequest.Headers.TryAddWithoutValidation(kv.Key, kv.Value);

                    try
                    {
                        var response = await _httpClient.SendAsync(cloudRequest, ct);
                        var content = await response.Content.ReadAsStringAsync(ct);
                        _logger?.LogInformation("WhatsAppCloud: response {StatusCode} for {To} body={Body}", response.StatusCode, to, content);

                        if (!response.IsSuccessStatusCode)
                        {
                            _logger?.LogError("WhatsAppCloud: error sending to {To}. Status={Status} Body={Body}", to, response.StatusCode, content);
                            throw new HttpRequestException($"WhatsAppCloud error: {response.StatusCode} - {content}");
                        }
                    }
                    catch (TaskCanceledException tce)
                    {
                        _logger?.LogError(tce, "WhatsAppCloud: request canceled for {To}", to);
                    }
                    catch (System.Exception ex)
                    {
                        _logger?.LogError(ex, "WhatsAppCloud: unexpected error sending to {To}", to);
                    }

                    continue; // next recipient
                }

                // default/custom provider payload
                payload = new
                {
                    number = to,
                    text = mensagem
                };

                payloadSerialized = JsonSerializer.Serialize(payload);
                _logger?.LogInformation("WhatsApp: sending to {To} endpoint={Endpoint} payload={Payload}", to, endpoint, payloadSerialized);

                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = JsonContent.Create(payload)
                };

                if (!string.IsNullOrWhiteSpace(_apiKey))
                {
                    if (string.Equals(_apiKeyHeader, "Authorization", StringComparison.OrdinalIgnoreCase))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue(_apiKeyScheme, _apiKey);
                    }
                    else
                    {
                        request.Headers.TryAddWithoutValidation(_apiKeyHeader, _apiKey);
                    }
                }

                // add any additional headers from config
                foreach (var kv in _additionalHeaders)
                    request.Headers.TryAddWithoutValidation(kv.Key, kv.Value);

                try
                {
                    var response = await _httpClient.SendAsync(request, ct);
                    var content = await response.Content.ReadAsStringAsync(ct);
                    _logger?.LogInformation("WhatsApp: response {StatusCode} for {To} body={Body}", response.StatusCode, to, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger?.LogError("WhatsApp: error sending to {To}. Status={Status} Body={Body}", to, response.StatusCode, content);
                        throw new HttpRequestException($"WhatsApp error: {response.StatusCode} - {content}");
                    }
                }
                catch (TaskCanceledException tce)
                {
                    _logger?.LogError(tce, "WhatsApp: request canceled for {To}", to);
                }
                catch (System.Exception ex)
                {
                    _logger?.LogError(ex, "WhatsApp: unexpected error sending to {To}", to);
                }
            }
        }
    }
}
