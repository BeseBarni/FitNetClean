using FitNetClean.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace FitNetClean.Domain.Entities;
public class Equipment : IDeletable
{
    [Required]
    public long Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    public long CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<Exercise> ExerciseList { get; set; } = new HashSet<Exercise>();
    public virtual ICollection<ContraIndication> ContraIndicationList { get; set; } = new HashSet<ContraIndication>();
    public bool IsDeleted { get; set; } = false;
}
