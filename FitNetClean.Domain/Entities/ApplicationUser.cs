using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FitNetClean.Domain.Entities;

public class ApplicationUser : IdentityUser<long>
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;
    
    [MaxLength(500)]
    [Url]
    public string? ProfilePictureUrl { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public virtual ICollection<FavoriteWorkout> FavoriteWorkouts { get; set; } = new HashSet<FavoriteWorkout>();
    public virtual ICollection<UserAvoidedContraIndication> AvoidedContraIndications { get; set; } = new HashSet<UserAvoidedContraIndication>();
}
