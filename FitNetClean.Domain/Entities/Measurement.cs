using FitNetClean.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FitNetClean.Domain.Entities;
public class Measurement
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Quantity must be a positive number")]
    public double Quantity { get; set; }
    
    [Required]
    public UnitOfMeasure Unit { get; set; }
}
