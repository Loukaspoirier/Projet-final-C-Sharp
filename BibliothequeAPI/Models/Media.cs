using System.ComponentModel.DataAnnotations;

namespace BibliothequeAPI.Models
{
    public class Media
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        public virtual string DisplayInformation()
        {
            return $"{Title} ({Author}) [{Type}]";
        }
    }
}
