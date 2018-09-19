using DDDEastAnglia.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDDEastAnglia.Api.Data {
    public class Db : DbContext {

        public Db(DbContextOptions<Db> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder
                .Entity<Category>()
                .HasData(
                    new Category { Id = 1, Title = "Category 1" }
                );

            modelBuilder
                .Entity<Product>()
                .HasData(
                    new { Id = 1, Title = "Product 1", CategoryId = 1 },
                    new { Id = 2, Title = "Product 2", CategoryId = 1 },
                    new { Id = 3, Title = "Product 3", CategoryId = 1 }
                );


            base.OnModelCreating(modelBuilder);
                
        }

    }
}
