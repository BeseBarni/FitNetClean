using FitNetClean.Domain.Constants;
using FitNetClean.Domain.Entities;
using FitNetClean.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitNetClean.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole<long>> roleManager)
    {
        string[] roleNames = { Roles.Admin, Roles.ProgramWriter, Roles.ProgramReader };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole<long>(roleName));
            }
        }
    }

    public static async Task SeedDefaultAdminAsync(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole<long>> roleManager,
        ILogger? logger = null)
    {
        // Ellenőrizzük, hogy létezik-e Admin szerepkörű felhasználó
        var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
        if (adminRole == null)
        {
            logger?.LogWarning("Admin role does not exist. Skipping default admin creation.");
            return;
        }

        var usersInAdminRole = await userManager.GetUsersInRoleAsync(Roles.Admin);
        
        if (usersInAdminRole.Any())
        {
            logger?.LogInformation("Admin user(s) already exist. Skipping default admin creation.");
            return;
        }

        // Default admin adatok
        var defaultAdmin = new ApplicationUser
        {
            UserName = "admin@fitnet.com",
            Email = "admin@fitnet.com",
            EmailConfirmed = true,
            FullName = "Default Admin",
            City = "Budapest",
            Country = "Hungary",
            CreatedAt = DateTime.UtcNow
        };

        const string defaultPassword = "Admin123!";

        var result = await userManager.CreateAsync(defaultAdmin, defaultPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin);
            logger?.LogInformation("Default admin user created successfully. Email: {Email}, Password: {Password}", 
                defaultAdmin.Email, defaultPassword);
        }
        else
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger?.LogError("Failed to create default admin user: {Errors}", errors);
        }
    }

    public static async Task SeedBasicEntitiesAsync(FitNetContext context)
    {
        // Csak akkor seedelünk, ha az edzésprogramok tábla üres
        if (await context.Workout.AnyAsync()) return;

        var cardio = new Category { Name = "Kardió", IsDeleted = false };
        var strength = new Category { Name = "Erősítő", IsDeleted = false };
        context.Category.AddRange(cardio, strength);

        var kneeProblem = new ContraIndication { Name = "Térdprobléma", IsDeleted = false };
        var highBP = new ContraIndication { Name = "Magas vérnyomás", IsDeleted = false };
        context.ContraIndication.AddRange(kneeProblem, highBP);

        var treadmill = new Equipment
        {
            Name = "Futópad",
            Category = cardio,
            IsDeleted = false,
            ContraIndicationList = new List<ContraIndication> { highBP }
        };
        var dumbell = new Equipment { Name = "Kézisúlyzó", Category = strength, IsDeleted = false };
        context.Equipment.AddRange(treadmill, dumbell);

        var workout = new Workout
        {
            CodeName = "FIT_BASIC_01",
            Title = "Kezdő Teljes Test",
            Description = "Alapozó edzés kezdőknek.",
            WarmupDurationMinutes = 10,
            MainWorkoutDurationMinutes = 45,
            IsDeleted = false
        };
        context.Workout.Add(workout);

        var warmupGroup = new WorkoutGroup { Title = "Bemelegítés", Workout = workout, IsDeleted = false };
        var mainGroup = new WorkoutGroup { Title = "Fő rész", Workout = workout, IsDeleted = false };
        context.WorkoutGroup.AddRange(warmupGroup, mainGroup);

        var runningMeasurement = new Measurement
        {
            Quantity = 10,
            Unit = UnitOfMeasure.Seconds
        };

        var running = new Exercise
        {
            Name = "Lassú kocogás",
            WorkoutGroup = warmupGroup,
            Workout = workout,
            Equipment = treadmill,
            IsDeleted = false,
            ContraIndicationList = new List<ContraIndication> { kneeProblem },
            Repetition = runningMeasurement
        };

        context.Exercise.Add(running);

        await context.SaveChangesAsync();
    }
}
