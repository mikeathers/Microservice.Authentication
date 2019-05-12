using Microservice.Authentication.Data.Models.Shared;
using Microservice.Authentication.Data.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Authentication.Data.Configurations
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) { base.OnModelCreating(builder); }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.EnableSensitiveDataLogging();

        public DbSet<Error> Errors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
    }
}
