namespace ShopQuanAo.Models
{
    public class CartModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discout { get; set; }
        public decimal Total { get { return Quantity * Price; } }
        public string Photo { get; set; }
        public CartModel() {
        
        }
        public CartModel(Product product)
        {
            ProductId=product.Id;
            ProductName = product.Name;
            Price = (decimal)product.Price;
            Quantity = 1;
            Photo=product.Photo;
            Discout = (decimal)product.Discout;

        }

    }
}
