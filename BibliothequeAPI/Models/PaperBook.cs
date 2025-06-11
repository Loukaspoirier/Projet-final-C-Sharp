using System.ComponentModel.DataAnnotations;

namespace BibliothequeAPI.Models
{
    public class PaperBook : Media
    {
        [Required]
        public int PageCount { get; set; }

        public override string DisplayInformation()
        {
            return $"{Title} ({Author}) [{PageCount} pages]";
        }
    }
}
