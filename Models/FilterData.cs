using static ShopQuanAo.Controllers.ProductsController;

namespace ShopQuanAo.Models
{
    public class FilterData
    {
        public List<string> PriceRange { get; set; }
        public List<string> Color { get; set; }
        public List<string> Size { get; set; }
    }
}
