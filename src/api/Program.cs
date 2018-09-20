using DDDEastAnglia.Api.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api {
    public class Program {
        public static async Task Main(string[] args) {

            var webHost = CreateWebHostBuilder(args).Build();

            // make sure we have one category for the demo
            using(var scope = webHost.Services.CreateScope()) {
                var db = scope.ServiceProvider.GetService<Db>();
                if(!await db.Categories.AnyAsync(x => x.Title == "Category 1")) {
                    db.Categories.Add(new Data.Entities.Category {
                        Title = "Category 1"
                    });
                    await db.SaveChangesAsync();
                }

            }

            await webHost.RunAsync();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
