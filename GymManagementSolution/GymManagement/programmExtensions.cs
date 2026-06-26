using GymManagement.DAL.Data.DataSeedin;
using GymManagement.DAL.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL
{
    public static class programmExtensions
    {
        public static async Task MigrationsAndSeedingDataAsync (this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Applying {pendingMigrations.Count()} Pending Migrations ");
                await dbContext.Database.MigrateAsync();
            }

            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "File");
            await GymDataSeeding.SeedingAsync(dbContext, seedPath, logger);
        }
    }
}
