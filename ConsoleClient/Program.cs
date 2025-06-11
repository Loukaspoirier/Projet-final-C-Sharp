using System.Net.Http.Json;
using System.Text.Json;

var options = new JsonSerializerOptions
{
    Converters = { new MediaJsonConverter() },
    PropertyNameCaseInsensitive = true,
    WriteIndented = true
};

HttpClient client = new();
client.BaseAddress = new Uri("http://localhost:5000");

try
{
    Console.WriteLine("Bibliothèque");
    Console.WriteLine("1 - Liste des élements");
    Console.WriteLine("2 - Rechercher un libre ou un Ebook");
    Console.WriteLine("2 - Ajouter un Ebook");
    Console.WriteLine("3 - Ajouter un livre");
    Console.WriteLine("4 - Supprimer un livre ou un Ebook");
    Console.Write("Choix : ");
    var choix = Console.ReadLine();

    switch (choix)
    {

        case "1":
            var medias = await client.GetFromJsonAsync<List<Media>>("livres", options);
            if (medias == null || medias.Count == 0)
            {
                Console.WriteLine("Aucun livre trouvé.");
                break;
            }
            break;

        case "2":
            Console.Write("Titre à rechercher (laisser vide pour ignorer) : ");
            string? searchTitle = Console.ReadLine();

            Console.Write("Auteur à rechercher (laisser vide pour ignorer) : ");
            string? searchAuthor = Console.ReadLine();

            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(searchTitle))
                queryParams.Add($"title={Uri.EscapeDataString(searchTitle)}");
            if (!string.IsNullOrWhiteSpace(searchAuthor))
                queryParams.Add($"author={Uri.EscapeDataString(searchAuthor)}");

            string queryString = queryParams.Count > 0 ? "livres?" + string.Join("&", queryParams) : "livres";

            var searchResults = await client.GetFromJsonAsync<List<Media>>(queryString, options);

            if (searchResults == null || searchResults.Count == 0)
            {
                Console.WriteLine("Aucun média trouvé avec ces critères.");
            }

            break;

        case "3":
            Console.Write("Titre de l'Ebook : ");
            string ebookTitle = Console.ReadLine() ?? "";

            Console.Write("Auteur de l'Ebook : ");
            string ebookAuthor = Console.ReadLine() ?? "";

            Console.Write("Format de fichier (ex : PDF) : ");
            string ebookFormat = Console.ReadLine() ?? "PDF";

            var newEbook = new Ebook
            {
                Title = ebookTitle,
                Author = ebookAuthor,
                FileFormat = ebookFormat,
                Type = "PDF"
            };

            var respEbook = await client.PostAsJsonAsync("livres/ebook", newEbook, options);
            Console.WriteLine(respEbook.IsSuccessStatusCode ? "Ebook ajouté." : $"Erreur: {respEbook.StatusCode}");
            break;

        case "4":
            Console.Write("Titre du livre : ");
            string paperTitle = Console.ReadLine() ?? "";

            Console.Write("Auteur du livre : ");
            string paperAuthor = Console.ReadLine() ?? "";

            int paperPageCount = 0;
            while (true)
            {
                Console.Write("Nombre de pages : ");
                string? pageCountInput = Console.ReadLine();
                if (int.TryParse(pageCountInput, out paperPageCount) && paperPageCount >= 0)
                    break;
                Console.WriteLine("Veuillez entrer un nombre valide.");
            }

            var newPaper = new PaperBook
            {
                Title = paperTitle,
                Author = paperAuthor,
                PageCount = paperPageCount,
                Type = "Papier"
            };

            var respPaper = await client.PostAsJsonAsync("livres/paper", newPaper, options);
            Console.WriteLine(respPaper.IsSuccessStatusCode ? "Livre ajouté." : $"Erreur: {respPaper.StatusCode}");
            break;

        case "5":
            Console.Write("Entrez l'ID du média à supprimer : ");
            string? inputId = Console.ReadLine();

            if (int.TryParse(inputId, out int idToDelete))
            {
                var response = await client.DeleteAsync($"livres/{idToDelete}");
                if (response.IsSuccessStatusCode)
                    Console.WriteLine($"Le média numéro {idToDelete} à supprimé avec succès.");
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    Console.WriteLine("Média introuvable.");
                else
                    Console.WriteLine($"Erreur lors de la suppression : {response.StatusCode}");
            }
            else
            {
                Console.WriteLine("ID invalide. Veuillez entrer un nombre entier.");
            }
            break;

        default:
            Console.WriteLine("Choix invalide.");
            break;
    }
}
catch (HttpRequestException httpEx)
{
    Console.WriteLine($"Erreur réseau : {httpEx.Message}");
}
catch (JsonException jsonEx)
{
    Console.WriteLine($"Erreur JSON : {jsonEx.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur inattendue : {ex.Message}");
}
