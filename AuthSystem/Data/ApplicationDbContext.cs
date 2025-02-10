using AuthSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProfileModel> Profiles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItemModel>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItemModel>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProfileModel>()
                .HasKey(p => p.userId);

            modelBuilder.Entity<ProfileModel>()
                .HasOne(p => p.User)
                .WithOne(ci => ci.Profile)
                .HasForeignKey<ProfileModel>(p => p.userId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderModel>()
           .Property(o => o.TotalAmount)
           .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}