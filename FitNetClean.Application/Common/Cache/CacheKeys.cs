namespace FitNetClean.Application.Common.Cache;

/// <summary>
/// Centralized cache key definitions
/// </summary>
public static class CacheKeys
{
    // ContraIndication cache keys
    public const string ContraIndicationPrefix = "contraindication";
    public const string AllContraIndications = "contraindication:all";
    public static string ContraIndicationById(long id) => $"contraindication:{id}";
    public static string ContraIndicationRelated(long id) => $"contraindication:{id}:related";
    public static string ContraIndicationEquipmentExercises(string ids) => $"contraindication:eq-ex:{ids}";

    // Equipment cache keys
    public const string EquipmentPrefix = "equipment";
    public const string AllEquipment = "equipment:all";
    public static string EquipmentById(long id) => $"equipment:{id}";
    public static string EquipmentByCategory(long categoryId) => $"equipment:category:{categoryId}";

    // Exercise cache keys
    public const string ExercisePrefix = "exercise";
    public const string AllExercises = "exercise:all";
    public static string ExerciseById(long id) => $"exercise:{id}";
    public static string ExerciseByWorkout(long workoutId) => $"exercise:workout:{workoutId}";

    // Category cache keys
    public const string CategoryPrefix = "category";
    public const string AllCategories = "category:all";
    public static string CategoryById(long id) => $"category:{id}";
}
