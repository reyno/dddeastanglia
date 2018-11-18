using Microsoft.EntityFrameworkCore;
using System;

namespace api.tests {
    public class DbContextHelper {

        public static TContext CreateInMemoryDbContext<TContext>()
            where TContext: DbContext {

            var dbContextOptions = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options
                ;

            return Activator.CreateInstance(
                typeof(TContext),
                new object[] { dbContextOptions }
                ) as TContext;

        }

    }
}
