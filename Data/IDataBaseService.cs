using Microsoft.EntityFrameworkCore;

namespace Data;

public interface IDataBaseService
{
    Task CriarBancoDeDados();
}

public class DataBaseService(ApplicationDbContext dbContext) : IDataBaseService
{
    public async Task CriarBancoDeDados()
    {
        var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
        var allMigrations = dbContext.Database.GetMigrations();

        var pendingMigrations = allMigrations.Except(appliedMigrations).ToList();

        if (pendingMigrations.Any())
        {
            // If there are pending migrations, apply them
            await dbContext.Database.MigrateAsync();
        }
    }
}