using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntitieFramework
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FruitkhaDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<ProductOrder> ProductOrders { get; set; }
    }
}
