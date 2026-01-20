using FitNetClean.Infrastructure.Extensions;
using FitNetClean.Infrastructure.Persistence;

namespace FitNetClean.WebAPI.Extensions;

public static class AppExtensions
{
    public static async Task UseApplyMigrations(this WebApplication app)
    {
        if (!(app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("APPLY_MIGRATIONS") == "true")) return;

        Console.WriteLine("Waiting for database to be ready...");
        var dbReady = await app.Services.WaitForDatabaseAsync();

        if (dbReady)
        {
            await app.Services.ApplyMigrationsAsync();
        }
        else
        {
            Console.WriteLine("WARNING: Could not connect to database. Application may not function correctly.");
        }
    }

    public static async Task UseDatabaseSeed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FitNetContext>();

        await DatabaseSeeder.SeedBasicEntitiesAsync(context);

    }
}
