using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopQuanAo.Infrastructure.Validation;

namespace ShopQuanAo.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(3000)]
        public string? Description { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? category { get; set; }
        [Column(TypeName ="decimal(8,2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(2,2)")]
        public decimal? Discout { get; set; }
        [StringLength(300)]
        public string? Photo { get; set; }
        [ForeignKey("Size")]
        public int SizeId { get; set; }
        public Size? size { get; set; }
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color? color { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsRecent { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpLoad { get; set; } 





    }
}
