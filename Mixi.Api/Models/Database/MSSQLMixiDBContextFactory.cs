using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mixi.Api.Modules.Database;

public class MSSQLMixiDBContextFactory : IDesignTimeDbContextFactory<MSSQLMixiDbContext>
{
    public MSSQLMixiDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<MSSQLMixiDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new MSSQLMixiDbContext(optionsBuilder.Options);
    }
}