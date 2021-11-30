using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string? Voornaam { get; set; }
        [Required]
        public string? Achternaam { get; set; }

        [Required]
        [DataType (DataType.Date)]
        public DateTime Geboortedatum { get; set; }

        [ForeignKey("Gender")]
        public char GeslachtId { get; set; }
        public Gender? Geslacht { get; set; }
    }
}
