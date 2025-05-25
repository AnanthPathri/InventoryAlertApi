using InventoryAlertApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAlertApi.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
        public DbSet<PRODUCTS> PRODUCTS { get; set; }
        public DbSet<STOCKBATCHES> STOCKBATCHES { get; set; }
        public DbSet<WAREHOUSES> WAREHOUSES { get; set; }

        public DbSet<CATEGORIES> CATEGORIES{ get; set; }
        public DbSet<NOTIFICATIONS> NOTIFICATIONS { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NOTIFICATIONS>().HasKey(n=>n.ID);
            //STOCKBATCHES -> PRODUCTS
            modelBuilder.Entity<STOCKBATCHES>()
                .HasOne(sb => sb.PRODUCTS)
                .WithMany(p => p.STOCKBATCHES)
                .HasForeignKey(sb => sb.PRODUCT_ID);
            //STOCKBATCHES -> WAREHOUSES
            modelBuilder.Entity<STOCKBATCHES>()
                .HasOne(sb => sb.WAREHOUSES)
                .WithMany(p => p.STOCKBATCHES)
                .HasForeignKey(sb => sb.WAREHOUSE_ID);
            modelBuilder.Entity<CATEGORIES>()
                .HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
    public class InventoryContext : DbContext
    {
        public DbSet<ALERTRULES> ALERTRULES { get; set; }
    }
}
