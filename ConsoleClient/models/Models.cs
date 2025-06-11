public class Media
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Type { get; set; } = "";

    public virtual string DisplayInformation() => $"{Title} ({Author}) [{Type}]";
}

public class Ebook : Media
{
    public string FileFormat { get; set; } = "PDF";

    public override string DisplayInformation() => $"{Title} ({Author}) [Format: {FileFormat}]";
}

public class PaperBook : Media
{
    public int PageCount { get; set; }

    public override string DisplayInformation() => $"{Title} ({Author}) [{PageCount} pages]";
}
