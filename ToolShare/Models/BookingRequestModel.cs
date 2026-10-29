using System.ComponentModel.DataAnnotations;
namespace ToolShare.Models;

public class BookingRequestModel : IValidatableObject
{
    [Required] public DateTime StartDate { get; set; } = DateTime.Today;
    [Required] public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);
    [StringLength(300)] public string? Message { get; set; }

    // custom validation: end must be after start
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (StartDate < DateTime.Today)
            yield return new ValidationResult("Start date cannot be in the past.", new[] { nameof(StartDate) });
        if (EndDate <= StartDate)
            yield return new ValidationResult("End date must be after start date.", new[] { nameof(EndDate) });
    }
}
