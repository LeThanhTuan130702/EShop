using System.ComponentModel.DataAnnotations;

namespace ShopQuanAo.Models
{
    public class Color
    {
        [Key]

        public int Id { get; set; }
        [StringLength(30)]

        public string? Name { get; set; }
    }
}
