using FitNetClean.Domain.Attributes;
using FitNetClean.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitNetClean.Domain.Entities;
public class Workout : IDeletable
{
    [Required]
    public long Id { get; set; }
    
    [Required]
    [WorkoutCodeName]
    public string CodeName { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Warmup duration must be 0 or greater")]
    public int WarmupDurationMinutes { get; set; } = 0;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Main workout duration must be greater than 0")]
    public int MainWorkoutDurationMinutes { get; set; }

    [NotMapped]
    public int TotalDurationMinutes => WarmupDurationMinutes + MainWorkoutDurationMinutes;

    public virtual ICollection<WorkoutGroup> WorkoutGroupList { get; set; } = new HashSet<WorkoutGroup>();
    public virtual ICollection<Exercise> ExerciseList { get; set; } = new HashSet<Exercise>();
    public virtual ICollection<FavoriteWorkout> FavoritedBy { get; set; } = new HashSet<FavoriteWorkout>();
    
    [Required]
    public bool IsDeleted { get; set; } = false;
}
