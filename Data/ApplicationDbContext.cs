using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopQuanAo.Models;

namespace ShopQuanAo.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Color> colors{ get; set; }
        public DbSet<Size> Sizes{ get; set; }
        public DbSet<OrderModel> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }





    }
}