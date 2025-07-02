using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DiamondShopSystem.DAL.Data
{
    public class DiamondShopDbContextFactory : IDesignTimeDbContextFactory<DiamondShopDbContext>
    {
        public DiamondShopDbContext CreateDbContext(string[] args)
        {
            DotNetEnv.Env.Load(Path.Combine("..", ".env")); // safer if relative to project structure

            var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            var host = isDocker
                ? Environment.GetEnvironmentVariable("DB_HOST") ?? "postgres"
                : Environment.GetEnvironmentVariable("DB_LOCAL_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            var db = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");

            //Console.WriteLine($"Host={host};Port={port};Database={db};Username={user};Password={(string.IsNullOrWhiteSpace(pass) ? "<empty>" : "***")}");
            Console.WriteLine($"Host={host};Port={port};Database={db};Username={user};Password={pass}");

            var optionsBuilder = new DbContextOptionsBuilder<DiamondShopDbContext>();
            var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass};Client Encoding=UTF8"; 

            optionsBuilder.UseNpgsql(connectionString);
            return new DiamondShopDbContext(optionsBuilder.Options);
        }
    }
}