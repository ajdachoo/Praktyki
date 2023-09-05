using System.ComponentModel.DataAnnotations;

namespace Zadanie1_MVC.Models
{
    public class Klient
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string? Pesel { get; set; }

        public int? BirthYear { get; set; }

        public int? Płeć { get; set; }
    }
}
