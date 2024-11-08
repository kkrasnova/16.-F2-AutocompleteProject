using System.ComponentModel.DataAnnotations;

namespace AutocompleteDemo.Models
{
    public class Item
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
    }
}