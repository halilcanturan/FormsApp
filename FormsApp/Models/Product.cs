using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models
{
    public class Product
    {
        [Display(Name = "Urun Id")]
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Urun Adı")]
        public string? Name { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Fiyat")]
        [Range(0,100000)]
        public decimal? Price { get; set; }

        [Required]
        [Display(Name = "Resim")]
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Category")]
        [Required]
        public int? CategoryId { get; set; }

    }
}
