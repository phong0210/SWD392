using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using DotNetEnv;

namespace DiamondShopSystem.DAL
{
    

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            DotNetEnv.Env.Load(Path.Combine("..", "..", ".env")); 
            var host = Environment.GetEnvironmentVariable("DB_LOCAL_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT_LOCAL");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var username = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(database) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("One or more required database environment variables are missing!");
            }

            var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};Client Encoding=UTF8;";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
} 