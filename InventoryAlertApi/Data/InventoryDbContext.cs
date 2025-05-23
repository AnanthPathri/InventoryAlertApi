using InventoryAlertApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAlertApi.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
        public DbSet<Products> Products { get; set; }
    }
}
