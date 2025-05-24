using InventoryAlertApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAlertApi.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
        public DbSet<PRODUCTS> PRODUCTS { get; set; }
        public DbSet<STOCKBATCHES> STOCKBATCHES { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<STOCKBATCHES>()
                .HasOne(sb => sb.PRODUCTS)
                .WithMany(p => p.STOCKBATCHES)
                .HasForeignKey(sb => sb.PRODUCT_ID);
            base.OnModelCreating(modelBuilder);
        }
    }
}
