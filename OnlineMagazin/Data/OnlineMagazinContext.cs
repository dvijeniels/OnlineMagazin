using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMagazin.Areas.Identity.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Data
{
    public class OnlineMagazinContext : IdentityDbContext<OnlineMagazinUser>
    { 
        public OnlineMagazinContext (DbContextOptions<OnlineMagazinContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Sliders> Sliders { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderLines> OrderLines { get; set; }
        public virtual DbSet<CategoryFeature> CategoryFeature { get; set; }
        public virtual DbSet<ProductFeatures> ProductFeatures { get; set; }
        public virtual DbSet<Duyrular> Duyrular { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<TypeProduct> TypeProducts { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<About> About { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<Mails> Mails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Products>().HasOne(m => m.TypeProduct).WithMany(m => m.Product).HasForeignKey(m => m.TypeId);
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.ApplyConfiguration(new OnlineMagazinUserEntityConfiguration());
        }

        public class OnlineMagazinUserEntityConfiguration : IEntityTypeConfiguration<OnlineMagazinUser>
        {
            public void Configure(EntityTypeBuilder<OnlineMagazinUser> builder)
            {
                
            }
        }
    }
}
