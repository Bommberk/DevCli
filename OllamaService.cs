namespace DevCli.Service;

using System.Net.Http.Json;

class OllamaService
{
    private readonly HttpClient _httpClient;

    public OllamaService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GenerateAsync(string prompt, string? input = null)
    {
        var request = new
        {
            model = "qwen3.5:9b",
            prompt,
            input,
            stream = false
        };

        var response = await _httpClient.PostAsJsonAsync(
            "http://localhost:11434/api/generate",
            request);

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

        return result?.Response ?? "";
    }
}

public class OllamaResponse
{
    public string Response { get; set; } = "";
}