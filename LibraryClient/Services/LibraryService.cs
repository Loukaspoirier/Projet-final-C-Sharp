using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class LibraryService
{
    private readonly HttpClient _httpClient;

    public LibraryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5000"); // Assurez-vous que l'URL est correcte
    }

    public async Task<List<Media>> GetMediaAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/livres");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<Media>>(responseBody, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération des médias: {ex.Message}");
            return new List<Media>();
        }
    }

    public async Task<Media> GetMediaByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/livres/{id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Media>(responseBody, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération du média: {ex.Message}");
            return null;
        }
    }

   public async Task<List<Media>> SearchMediaAsync(string authorQuery, string titleQuery)
    {
        try
        {
            var url = "/livres";
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(authorQuery))
            {
                queryParams.Add($"author={Uri.EscapeDataString(authorQuery)}");
            }

            if (!string.IsNullOrWhiteSpace(titleQuery))
            {
                queryParams.Add($"title={Uri.EscapeDataString(titleQuery)}");
            }

            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<Media>>(responseBody, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la recherche des médias: {ex.Message}");
            return new List<Media>();
        }
    }




    public async Task AddEbookAsync(Ebook ebook)
    {
        try
        {
            var ebookJson = JsonSerializer.Serialize(ebook);
            var content = new StringContent(ebookJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/livres/ebook", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout du livre électronique: {ex.Message}");
        }
    }

    public async Task AddPaperBookAsync(PaperBook paperBook)
    {
        try
        {
            var paperBookJson = JsonSerializer.Serialize(paperBook);
            var content = new StringContent(paperBookJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/livres/paper", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout du livre papier: {ex.Message}");
        }
    }

    public async Task UpdateMediaAsync(int id, Media media)
    {
        try
        {
            var mediaJson = JsonSerializer.Serialize(media);
            var content = new StringContent(mediaJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/livres/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour du média: {ex.Message}");
        }
    }

    public async Task DeleteMediaAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/livres/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression du média: {ex.Message}");
        }
    }
}
