using Microsoft.EntityFrameworkCore;
using ToolShare.Models;
namespace ToolShare.Services;

public interface IBookingService
{
    Task<string?> RequestAsync(int itemId, int borrowerId, BookingRequestModel m);
    Task<List<Booking>> GetByBorrowerAsync(int memberId);
    Task<List<Booking>> GetIncomingAsync(int ownerId);
    Task<string?> ApproveAsync(int id);
    Task<string?> RejectAsync(int id);
    Task<string?> CancelAsync(int id);
    Task<string?> MarkReturnedAsync(int id);
}

// every method returns null on success, or an error message
public class BookingService(IDbContextFactory<AppDb> factory) : IBookingService
{
    public async Task<string?> RequestAsync(int itemId, int borrowerId, BookingRequestModel m)
    {
        await using var db = await factory.CreateDbContextAsync();
        var item = await db.Items.FindAsync(itemId);
        if (item is null) return "Item not found.";
        if (item.OwnerId == borrowerId) return "You can't borrow your own item.";
        if (!item.IsAvailable) return "This item is not available right now.";
        db.Bookings.Add(new Booking(itemId, borrowerId, m.StartDate, m.EndDate, m.Message));
        await db.SaveChangesAsync();
        return null;
    }

    public async Task<List<Booking>> GetByBorrowerAsync(int memberId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Bookings.Include(b => b.Item)
            .Where(b => b.BorrowerId == memberId)
            .OrderByDescending(b => b.StartDate).ToListAsync();
    }

    public async Task<List<Booking>> GetIncomingAsync(int ownerId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Bookings.Include(b => b.Item).Include(b => b.Borrower)
            .Where(b => b.Item!.OwnerId == ownerId)
            .OrderByDescending(b => b.StartDate).ToListAsync();
    }

    public Task<string?> ApproveAsync(int id) => Change(id, b => { b.Approve(); b.Item!.MarkReserved(); });
    public Task<string?> RejectAsync(int id) => Change(id, b => b.Reject());
    public Task<string?> CancelAsync(int id) => Change(id, b => { b.Cancel(); b.Item!.MarkAvailable(); });
    public Task<string?> MarkReturnedAsync(int id) => Change(id, b => { b.MarkReturned(); b.Item!.MarkAvailable(); });

    private async Task<string?> Change(int id, Action<Booking> action)
    {
        await using var db = await factory.CreateDbContextAsync();
        var b = await db.Bookings.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        if (b is null) return "Booking not found.";
        try { action(b); await db.SaveChangesAsync(); return null; }
        catch (InvalidOperationException ex) { return ex.Message; }
    }
}
