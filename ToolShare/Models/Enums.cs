namespace ToolShare.Models;

public enum ItemStatus { Available, Reserved }
public enum BookingStatus { Pending, Approved, Rejected, Cancelled, Completed }

public static class Categories
{
    public static readonly string[] All =
        { "Power Tools", "Cameras", "Camping Gear", "Sports Equipment", "DIY Tools", "Others" };
}
