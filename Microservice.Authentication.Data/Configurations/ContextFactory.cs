using System;
using System.IO;
using Microservice.Authentication.Data.Models.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microservice.Authentication.Data.Configurations
{
    public class ContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private readonly DatabaseSettings _dbSettings;

        public ContextFactory(IOptions<DatabaseSettings> options)
        {
            _dbSettings = options.Value;
        }

        public ContextFactory() { }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Microservice.Authentication.Api"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();


            var assembly = config["DatabaseSettings:DefaultAssembly"];
            var connectionString = config["DatabaseSettings:ConnectionString"];
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, migrations => migrations.MigrationsAssembly(assembly));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}