namespace ToolShare.Models;

public class Member : Person   // inheritance
{
    public string Location { get; set; } = "";
    public List<EquipmentItem> Items { get; set; } = new();
}
