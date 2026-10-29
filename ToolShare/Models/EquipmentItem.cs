using System.ComponentModel.DataAnnotations;
namespace ToolShare.Models;

public class EquipmentItem : IBookable
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")] public string Name { get; set; } = "";
    [Required] public string Category { get; set; } = "Power Tools";
    [Required(ErrorMessage = "Location is required")] public string Location { get; set; } = "";
    public string Description { get; set; } = "";
    public string? Conditions { get; set; }
    public string? ImageUrl { get; set; }

    // encapsulation: outside code can't set Status directly
    public ItemStatus Status { get; private set; } = ItemStatus.Available;

    public int OwnerId { get; set; }
    public Member? Owner { get; set; }

    public bool IsAvailable => Status == ItemStatus.Available;

    public void MarkReserved()
    {
        if (!IsAvailable) throw new InvalidOperationException("Item is already reserved.");
        Status = ItemStatus.Reserved;
    }
    public void MarkAvailable() => Status = ItemStatus.Available;
}
