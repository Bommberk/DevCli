using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DevCli.Service;

public class OllamaService
{
    private readonly HttpClient _httpClient;

    public OllamaService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GenerateAsync(string prompt, string? input = null)
    {
        var fullPrompt = string.IsNullOrWhiteSpace(input)
            ? prompt
            : $"{prompt}\n\n{input}";

        ConsoleWarning($"full Promt lenght: {fullPrompt.Length}");

        var request = new
        {
            model = "qwen3.5:9b",
            prompt = fullPrompt,
            stream = false,
            think = false,
            options = new
            {
                num_ctx = 32768,
                num_predict = 2048
            }
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:11434/api/generate",
                request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
            var rawResponse = await response.Content.ReadAsStringAsync();
            writeFile("ollama_raw_response.json", rawResponse, true);

            if (result == null)
            {
                return "Ollama returned no response.";
            }

            OllamaInfo(result, fullPrompt);
            return result.Response ?? "";
        }
        catch (Exception ex)
        {
            return $"Ollama Error: {ex.Message}";
        }
    }
    private void OllamaInfo(OllamaResponse response, dynamic diff)
    {
        Console.ForegroundColor = ConsoleColor.White;
        ConsoleLog($"Response Length: {response.Response.Length}");
        ConsoleLog($"Prompt Tokens: {response.PromptEvalCount}");
        ConsoleLog($"Generated Tokens: {response.EvalCount}");
        ConsoleLog($"Done Reason: {response.DoneReason}");
        Console.ResetColor();
    }

public class OllamaResponse
{
    public string Response { get; set; } = "";
    public string Thinking { get; set; } = "";

    [JsonPropertyName("prompt_eval_count")]
    public int PromptEvalCount { get; set; }

    [JsonPropertyName("eval_count")]
    public int EvalCount { get; set; }

    [JsonPropertyName("done_reason")]
    public string DoneReason { get; set; } = ""; }
}