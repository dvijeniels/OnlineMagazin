using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Models;
using OnlineShop.Models;

namespace OnlineMagazin.Data
{
    public class OnlineMagazinContext : DbContext
    {
        public OnlineMagazinContext (DbContextOptions<OnlineMagazinContext> options)
            : base(options)
        {
            
        }
        public virtual DbSet<Sliders> Sliders { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<CategoryFeature> CategoryFeature { get; set; }
        public virtual DbSet<ProductFeatures> ProductFeatures { get; set; }
        public virtual DbSet<Duyrular> Duyrular { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<About> About { get; set; }
        public virtual DbSet<Uye> Uye { get; set; }
        public virtual DbSet<Comments> Comment { get; set; }
    }
}
