using ClientConsole.Models;
using ClientConsole.Services;

var api = new ApiService();

while (true)
{
    Console.WriteLine(" Menu Bibliothèque");
    Console.WriteLine("1. Lister tous les livres");
    Console.WriteLine("2. Rechercher des livres");
    Console.WriteLine("3. Voir un livre par ID");
    Console.WriteLine("4. Ajouter un ebook ou un livre papier");
    Console.WriteLine("5. Modifier un livre");
    Console.WriteLine("6. Supprimer un livre");
    Console.WriteLine("0. Quitter");
    Console.Write("Choix : ");
    var choix = Console.ReadLine();

    switch (choix)
    {
        case "1":
            await ListerTousLesLivres(api);
            break;
        case "2":
            await RechercherLivres(api);
            break;
        case "3":
            await VoirLivre(api);
            break;
        case "4":
            await AjouterLivre(api);
            break;
        case "5":
            await ModifierLivre(api);
            break;
        case "6":
            await SupprimerLivre(api);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Option invalide.");
            break;
    }
}

static async Task ListerTousLesLivres(ApiService api)
{
    var livres = await api.GetAllMediaAsync();
    foreach (var livre in livres)
    {
        Console.WriteLine($"{livre.Id} - {livre.Title} ({livre.Author}) [{livre.Type}]");
    }
}

static async Task RechercherLivres(ApiService api)
{
    Console.Write("Filtrer par auteur (laisser vide si aucun) : ");
    var author = Console.ReadLine();
    Console.Write("Filtrer par titre (laisser vide si aucun) : ");
    var title = Console.ReadLine();

    var livres = await api.GetAllMediaAsync(author, title);
    foreach (var livre in livres)
    {
        Console.WriteLine($"{livre.Id} - {livre.Title} ({livre.Author}) [{livre.Type}]");
    }
}

static async Task VoirLivre(ApiService api)
{
    Console.Write("ID à consulter : ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID invalide.");
        return;
    }

    var livre = await api.GetByIdAsync(id);
    if (livre == null)
    {
        Console.WriteLine("Livre non trouvé.");
        return;
    }

    Console.WriteLine($"Titre : {livre.Title}");
    Console.WriteLine($"Auteur : {livre.Author}");
    Console.WriteLine($"Type : {livre.Type}");

    if (livre is Ebook ebook)
        Console.WriteLine($"Format : {ebook.Format}");
    else if (livre is PaperBook paper)
        Console.WriteLine($"Nombre de pages : {paper.PageCount}");
}

static async Task AjouterLivre(ApiService api)
{
    Console.Write("Type : 1=Ebook, 2=Papier\n");
    var type = Console.ReadLine();

    Console.Write("Titre : ");
    var title = Console.ReadLine();
    Console.Write("Auteur : ");
    var author = Console.ReadLine();

    if (type == "1")
    {
        Console.Write("Format (PDF/EPUB) : ");
        var format = Console.ReadLine();

        var ebook = new Ebook { Title = title!, Author = author!, Type = "ebook", Format = format! };
        if (await api.AddEbookAsync(ebook))
            Console.WriteLine("Ebook ajouté !");
        else
            Console.WriteLine("Échec de l'ajout.");
    }
    else if (type == "2")
    {
        Console.Write("Nombre de pages : ");
        if (!int.TryParse(Console.ReadLine(), out int pages))
        {
            Console.WriteLine("Nombre invalide.");
            return;
        }

        var paper = new PaperBook { Title = title!, Author = author!, Type = "paper", PageCount = pages };
        if (await api.AddPaperBookAsync(paper))
            Console.WriteLine("Livre papier ajouté !");
        else
            Console.WriteLine("Échec de l'ajout.");
    }
    else
    {
        Console.WriteLine("Type inconnu.");
    }
}

static async Task ModifierLivre(ApiService api)
{
    Console.Write("ID à modifier : ");
    if (!int.TryParse(Console.ReadLine(), out int id)) return;

    var livre = await api.GetByIdAsync(id);
    if (livre == null)
    {
        Console.WriteLine("Livre non trouvé.");
        return;
    }

    Console.Write($"Titre ({livre.Title}) : ");
    var newTitle = Console.ReadLine();
    Console.Write($"Auteur ({livre.Author}) : ");
    var newAuthor = Console.ReadLine();

    livre.Title = string.IsNullOrWhiteSpace(newTitle) ? livre.Title : newTitle!;
    livre.Author = string.IsNullOrWhiteSpace(newAuthor) ? livre.Author : newAuthor!;

    if (livre is Ebook ebook)
    {
        Console.Write($"Format ({ebook.Format}) : ");
        var newFormat = Console.ReadLine();
        ebook.Format = string.IsNullOrWhiteSpace(newFormat) ? ebook.Format : newFormat!;
    }
    else if (livre is PaperBook paper)
    {
        Console.Write($"Pages ({paper.PageCount}) : ");
        var pagesInput = Console.ReadLine();
        if (int.TryParse(pagesInput, out int newPages))
            paper.PageCount = newPages;
    }

    if (await api.UpdateAsync(id, livre))
        Console.WriteLine("Modification réussie !");
    else
        Console.WriteLine("Échec de la modification.");
}

static async Task SupprimerLivre(ApiService api)
{
    Console.Write("ID à supprimer : ");
    if (!int.TryParse(Console.ReadLine(), out int id)) return;

    if (await api.DeleteAsync(id))
        Console.WriteLine("Suppression réussie !");
    else
        Console.WriteLine("Échec de la suppression.");
}
