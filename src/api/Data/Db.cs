using DDDEastAnglia.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDDEastAnglia.Api.Data {
    public class Db : DbContext {

        public Db(DbContextOptions<Db> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
