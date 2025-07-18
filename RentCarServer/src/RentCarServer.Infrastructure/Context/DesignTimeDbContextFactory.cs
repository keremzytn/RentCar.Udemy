using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RentCarServer.Infrastructure.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))!)
            .AddJsonFile("src/RentCarServer.WebAPI/appsettings.Development.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("PostgreSQL");

        builder.UseNpgsql(connectionString);

        return new ApplicationDbContext(builder.Options);
    }
}