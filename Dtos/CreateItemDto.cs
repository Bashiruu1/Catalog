using System.ComponentModel.DataAnnotations;

namespace Catalog.Dtos
{   
    public class CreateitemDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }
    }
}