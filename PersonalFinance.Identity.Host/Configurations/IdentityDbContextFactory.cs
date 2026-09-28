using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Identity.Infrastructure.Configurations;

namespace PersonalFinance.Identity.Host.Configurations
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<IdentityDbContextFactory>()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("A connection string 'DefaultConnection' não foi configurada.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}