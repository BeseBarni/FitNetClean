using FitNetClean.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitNetClean.Domain.Entities;
public class Workout : IDeletable
{
    [Required]
    public long Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string CodeName { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int WarmupDurationMinutes { get; set; } = 0;

    [Required]
    public int MainWorkoutDurationMinutes { get; set; }

    [NotMapped]
    public int TotalDurationMinutes => WarmupDurationMinutes + MainWorkoutDurationMinutes;

    public virtual ICollection<WorkoutGroup> WorkoutGroupList { get; set; } = new HashSet<WorkoutGroup>();
    public virtual ICollection<Exercise> ExerciseList { get; set; } = new HashSet<Exercise>();
    public bool IsDeleted { get; set; } = false;
}
