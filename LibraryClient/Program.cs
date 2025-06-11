using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var libraryService = new LibraryService(httpClient);

        while (true)
        {
            Console.WriteLine("1. Afficher la liste des livres");
            Console.WriteLine("2. Rechercher un livre");
            Console.WriteLine("3. Ajouter un livre électronique");
            Console.WriteLine("4. Ajouter un livre papier");
            Console.WriteLine("5. Modifier un livre");
            Console.WriteLine("6. Supprimer un livre");
            Console.WriteLine("7. Quitter");
            Console.Write("Choisissez une option: ");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await DisplayBooks(libraryService);
                    break;
                case "2":
                    await SearchBooks(libraryService);
                    break;
                case "3":
                    await AddEbook(libraryService);
                    break;
                case "4":
                    await AddPaperBook(libraryService);
                    break;
                case "5":
                    await UpdateBook(libraryService);
                    break;
                case "6":
                    await DeleteBook(libraryService);
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Option invalide.");
                    break;
            }
        }
    }

    static async Task DisplayBooks(LibraryService libraryService)
    {
        var mediaList = await libraryService.GetMediaAsync();
        foreach (var media in mediaList)
        {
            Console.WriteLine($"ID: {media.Id}, Titre: {media.Title}, Auteur: {media.Author}");
        }
    }

    static async Task SearchBooks(LibraryService libraryService)
    {
        Console.Write("Entrez le titre à rechercher: ");
        var titleQuery = Console.ReadLine();

        Console.Write("Entrez l'auteur à rechercher: ");
        var authorQuery = Console.ReadLine();

        var mediaList = await libraryService.SearchMediaAsync(authorQuery, titleQuery);

        if (mediaList.Any())
        {
            foreach (var media in mediaList)
            {
                Console.WriteLine($"ID: {media.Id}, Titre: {media.Title}, Auteur: {media.Author}");
            }
        }
        else
        {
            Console.WriteLine("Aucun livre trouvé.");
        }
    }




    static async Task AddEbook(LibraryService libraryService)
    {
        var ebook = new Ebook();
        Console.Write("Titre du livre électronique: ");
        ebook.Title = Console.ReadLine();
        Console.Write("Auteur du livre électronique: ");
        ebook.Author = Console.ReadLine();
        ebook.Type = "Ebook";
        ebook.Format = "PDF";

        await libraryService.AddEbookAsync(ebook);
        Console.WriteLine("Livre électronique ajouté avec succès.");
    }

    static async Task AddPaperBook(LibraryService libraryService)
    {
        var paperBook = new PaperBook();
        Console.Write("Titre du livre papier: ");
        paperBook.Title = Console.ReadLine();
        Console.Write("Auteur du livre papier: ");
        paperBook.Author = Console.ReadLine();
        paperBook.Type = "PaperBook";

        await libraryService.AddPaperBookAsync(paperBook);
        Console.WriteLine("Livre papier ajouté avec succès.");
    }

    static async Task UpdateBook(LibraryService libraryService)
    {
        Console.Write("Entrez l'ID du livre à modifier: ");
        var id = int.Parse(Console.ReadLine());
        var media = await libraryService.GetMediaByIdAsync(id);

        if (media != null)
        {
            Console.Write("Nouveau titre du livre: ");
            media.Title = Console.ReadLine();
            Console.Write("Nouvel auteur du livre: ");
            media.Author = Console.ReadLine();

            await libraryService.UpdateMediaAsync(id, media);
            Console.WriteLine("Livre mis à jour avec succès.");
        }
        else
        {
            Console.WriteLine("Livre non trouvé.");
        }
    }

    static async Task DeleteBook(LibraryService libraryService)
    {
        Console.Write("Entrez l'ID du livre à supprimer: ");
        var id = int.Parse(Console.ReadLine());
        await libraryService.DeleteMediaAsync(id);
        Console.WriteLine("Livre supprimé avec succès.");
    }
}
