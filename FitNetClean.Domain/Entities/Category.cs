using FitNetClean.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace FitNetClean.Domain.Entities;
public class Category : IDeletable
{
    [Required]
    public long Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Equipment> EquipmentList { get; set; } = new HashSet<Equipment>();

    [Required]
    public bool IsDeleted { get; set; } = false;
}
