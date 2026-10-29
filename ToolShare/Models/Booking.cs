namespace ToolShare.Models;

public class Booking
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public EquipmentItem? Item { get; set; }
    public int BorrowerId { get; set; }
    public Member? Borrower { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Message { get; set; }
    public BookingStatus Status { get; private set; } = BookingStatus.Pending;

    protected Booking() { }   // needed by EF
    public Booking(int itemId, int borrowerId, DateTime start, DateTime end, string? message)
    {
        ItemId = itemId; BorrowerId = borrowerId;
        StartDate = start; EndDate = end; Message = message;
    }

    // status changes only through these methods, each checks the rules
    public void Approve()
    {
        if (Status != BookingStatus.Pending) throw new InvalidOperationException("Only pending requests can be approved.");
        Status = BookingStatus.Approved;
    }
    public void Reject()
    {
        if (Status != BookingStatus.Pending) throw new InvalidOperationException("Only pending requests can be rejected.");
        Status = BookingStatus.Rejected;
    }
    public void Cancel()
    {
        if (Status is not (BookingStatus.Pending or BookingStatus.Approved))
            throw new InvalidOperationException("This booking can't be cancelled.");
        Status = BookingStatus.Cancelled;
    }
    public void MarkReturned()
    {
        if (Status != BookingStatus.Approved) throw new InvalidOperationException("Only approved bookings can be returned.");
        Status = BookingStatus.Completed;
    }
}
