using System.Text.Json;
using System.Text.Json.Serialization;

public class MediaJsonConverter : JsonConverter<Media>
{
public override Media Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
{
    using var jsonDoc = JsonDocument.ParseValue(ref reader);
    var root = jsonDoc.RootElement;

    Console.WriteLine("");
        foreach (var prop in root.EnumerateObject())
        {
            if (prop.NameEquals("id"))
                continue;

            Console.WriteLine($"  {prop.Name} : {prop.Value}");
        }
        

    if (!root.TryGetProperty("type", out var typeProp))
        {
            Console.WriteLine("ATTENTION : la propriété 'type' est absente. Je retourne Media simple.");
            return JsonSerializer.Deserialize<Media>(root.GetRawText(), options)!;
        }
    
    
    var type = typeProp.GetString()?.ToLower();
    return type switch
    {
        "pdf" => JsonSerializer.Deserialize<Ebook>(root.GetRawText(), options)!,
        "papier" => JsonSerializer.Deserialize<PaperBook>(root.GetRawText(), options)!,
        _ => JsonSerializer.Deserialize<Media>(root.GetRawText(), options)!,
    };
}

    public override void Write(Utf8JsonWriter writer, Media value, JsonSerializerOptions options)
    {
        var typeName = value switch
        {
            Ebook => "Ebook",
            PaperBook => "PaperBook",
            _ => "Media"
        };

        using var doc = JsonDocument.Parse(JsonSerializer.Serialize(value, value.GetType(), options));
        writer.WriteStartObject();
        writer.WriteString("$type", typeName);

        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            prop.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}