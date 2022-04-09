using KitchenManager.API.Data.Seed;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace KitchenManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            if (args.Length == 1 && args[0].ToLower() == "seed")
            {
                RunSeeding(host);
            }
            host.Run();
        }
        
        public static void RunSeeding(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<KMSeeder>();
                seeder.RootPath = Directory.GetCurrentDirectory();

                bool clearData = true;

                seeder.SeedDataAsync(clearData).Wait();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
