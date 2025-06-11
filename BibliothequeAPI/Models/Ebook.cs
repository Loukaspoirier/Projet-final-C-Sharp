using System.ComponentModel.DataAnnotations;

namespace BibliothequeAPI.Models
{
    public class Ebook : Media
    {
        [Required]
        public string FileFormat { get; set; } = "PDF";

        public override string DisplayInformation()
        {
            return $"{Title} ({Author}) [{FileFormat}]";
        }
    }
}
