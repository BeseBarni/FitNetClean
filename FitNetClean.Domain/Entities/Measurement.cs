using FitNetClean.Domain.Enums;

namespace FitNetClean.Domain.Entities;
public class Measurement
{
    public double Quantity { get; set; }
    public UnitOfMeasure Unit { get; set; }
}
