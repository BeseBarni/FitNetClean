using System.ComponentModel.DataAnnotations;
using FitNetClean.Domain.Common;

namespace FitNetClean.Domain.Entities;

public class UserAvoidedContraIndication : IDeletable
{
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public long ContraIndicationId { get; set; }
    
    [Required]
    public DateTime MarkedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public bool IsDeleted { get; set; } = false;
    
    public virtual ApplicationUser User { get; set; } = null!;
    
    public virtual ContraIndication ContraIndication { get; set; } = null!;
}
