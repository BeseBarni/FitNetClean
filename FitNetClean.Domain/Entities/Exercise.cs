using FitNetClean.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace FitNetClean.Domain.Entities;
public class Exercise : IDeletable
{
    [Required]
    public long Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;
    public Measurement Repetition { get; set; } = null!;

    [Required]
    public long WorkoutId { get; set; }
    public virtual Workout Workout { get; set; } = null!;
    public long? WorkoutGroupId { get; set; }
    public virtual WorkoutGroup? WorkoutGroup { get; set; }
    public long? EquipmentId { get; set; }
    public virtual Equipment? Equipment { get; set; }
    public virtual ICollection<ContraIndication> ContraIndicationList { get; set; } = new HashSet<ContraIndication>();
    public bool IsDeleted { get; set; } = false;
}
