using System.Text;
using System.Text.Json;
using ClientConsole.Models;

namespace ClientConsole.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5000/livres";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Media>> GetAllMediaAsync(string? author = null, string? title = null)
    {
        var query = new List<string>();
        if (!string.IsNullOrEmpty(author))
            query.Add($"author={Uri.EscapeDataString(author)}");
        if (!string.IsNullOrEmpty(title))
            query.Add($"title={Uri.EscapeDataString(title)}");

        var url = _baseUrl;
        if (query.Any())
            url += "?" + string.Join("&", query);

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Media>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
    }

    public async Task<Media?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Media>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> AddEbookAsync(Ebook ebook)
    {
        var json = JsonSerializer.Serialize(ebook);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/ebook", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddPaperBookAsync(PaperBook paperBook)
    {
        var json = JsonSerializer.Serialize(paperBook);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/paper", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, Media updated)
    {
        var json = JsonSerializer.Serialize(updated);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_baseUrl}/{id}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
}
