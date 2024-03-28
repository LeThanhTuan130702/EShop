using System.ComponentModel.DataAnnotations;

namespace ShopQuanAo.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        public string? Name { get; set; }

    }
}
