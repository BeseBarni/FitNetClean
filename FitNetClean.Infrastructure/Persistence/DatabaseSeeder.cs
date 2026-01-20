using FitNetClean.Domain.Entities;
using FitNetClean.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Infrastructure.Persistence;

public static class DatabaseSeeder
{
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
