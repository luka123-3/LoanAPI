using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Get the startup project directory (WebApplication2)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..");
            if (!Directory.Exists(Path.Combine(basePath, "WebApplication2")))
            {
                basePath = Directory.GetCurrentDirectory();
            }
            else
            {
                basePath = Path.Combine(basePath, "WebApplication2");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
