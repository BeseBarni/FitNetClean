using Microsoft.AspNetCore.Identity;

namespace FitNetClean.Domain.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public string FullName { get; set; } = string.Empty;
    
    public string City { get; set; } = string.Empty;
    
    public string Country { get; set; } = string.Empty;
    
    public string? ProfilePictureUrl { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
}
