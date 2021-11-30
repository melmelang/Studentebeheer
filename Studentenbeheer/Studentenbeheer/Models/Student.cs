using System.ComponentModel.DataAnnotations;

namespace Studentenbeheer.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }

        [DataType (DataType.Date)]
        public DateTime Geboortedatum { get; set; }
        public char Geslacht { get; set; }
    }
}
