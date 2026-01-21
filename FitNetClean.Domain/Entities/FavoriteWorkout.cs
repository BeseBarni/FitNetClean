using System.ComponentModel.DataAnnotations;
using FitNetClean.Domain.Common;

namespace FitNetClean.Domain.Entities;

public class FavoriteWorkout : IDeletable
{
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public long WorkoutId { get; set; }
    
    [Required]
    public DateTime FavoritedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public bool IsDeleted { get; set; } = false;
    
    public virtual ApplicationUser User { get; set; } = null!;
    
    public virtual Workout Workout { get; set; } = null!;
}
