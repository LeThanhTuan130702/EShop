using System.ComponentModel.DataAnnotations.Schema;

namespace ShopQuanAo.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }
        //[ForeignKey("Product")]

        public int ProductId { get; set; }
        //public Product Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        //[ForeignKey("OrderModel")]
        //public int OrderId { get; set; }
        //public OrderModel OrderModel { get; set; }

    }
}
