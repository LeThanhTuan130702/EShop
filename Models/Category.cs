using System.ComponentModel.DataAnnotations;
using ShopQuanAo.Infrastructure.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopQuanAo.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]

        public string Name { get; set; }
        [StringLength(300)]

        public string? Photo { get; set; }
        public int CategoryOrders { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpLoadCate { get; set; }
        //public int Status { get; set; }
    }
}