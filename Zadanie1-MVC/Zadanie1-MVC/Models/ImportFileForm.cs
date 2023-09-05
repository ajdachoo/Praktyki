using System.ComponentModel.DataAnnotations;

namespace Zadanie1_MVC.Models
{
    public class ImportFileForm
    {
        [Required]
        public IFormFile FormFile { get; set; }
    }
}
