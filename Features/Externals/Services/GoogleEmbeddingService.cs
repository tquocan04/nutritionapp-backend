using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Features.Externals.Services
{
    public class GoogleEmbeddingService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        ILogger<GoogleEmbeddingService> logger)
        : IEmbeddingService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _apiKey = configuration["GoogleAI:ApiKey"] ?? throw new InvalidOperationException("Google AI ApiKey must be configured.");
        private readonly string _modelName = "embedding-001"; // Model embedding of Google
        private readonly ILogger<GoogleEmbeddingService> _logger = logger;

        public async Task<float[]?> GetEmbeddingAsync(string textToEmbed)
        {
            if (string.IsNullOrWhiteSpace(textToEmbed)) return null;

            // URL with API Key
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_modelName}:embedContent?key={_apiKey}";

            // body request
            var requestBody = new
            {
                content = new
                {
                    parts = new[] { new { text = textToEmbed } }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                // POST
                var response = await httpClient.PostAsJsonAsync(url, requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Lỗi từ Google Gemini API: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
                    return null;
                }

                // read và parse response
                var embeddingResponse = await response.Content.ReadFromJsonAsync<GeminiEmbeddingResponse>();
                Console.WriteLine($"Kq trả về: {embeddingResponse?.Embedding?.Values}");
                return embeddingResponse?.Embedding?.Values;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi Google Gemini Embedding API cho text: '{Text}'", textToEmbed);
                return null;
            }
        }

        private record GeminiEmbeddingResponse([property: JsonPropertyName("embedding")] GeminiEmbedding Embedding);
        private record GeminiEmbedding([property: JsonPropertyName("values")] float[] Values);

    }
}