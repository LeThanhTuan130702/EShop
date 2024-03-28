using System.ComponentModel.DataAnnotations.Schema;

namespace ShopQuanAo.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Order_Code { get; set; }
        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
       
    }
}
