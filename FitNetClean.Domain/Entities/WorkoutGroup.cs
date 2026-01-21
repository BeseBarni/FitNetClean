using FitNetClean.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace FitNetClean.Domain.Entities;
public class WorkoutGroup : IDeletable
{
    [Required]
    public long Id { get; set; }

    [Required]
    public long WorkoutId { get; set; }
    public virtual Workout Workout { get; set; } = null!;

    public virtual ICollection<Exercise> ExerciseList { get; set; } = new HashSet<Exercise>();

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;
    
    [Required]
    public bool IsDeleted { get; set; } = false;
}
