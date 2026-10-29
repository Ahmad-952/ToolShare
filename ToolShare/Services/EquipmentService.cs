using Microsoft.EntityFrameworkCore;
using ToolShare.Models;
namespace ToolShare.Services;

public interface IEquipmentService
{
    Task<List<EquipmentItem>> GetAllAsync();
    Task<EquipmentItem?> GetByIdAsync(int id);
    Task<List<EquipmentItem>> GetByOwnerAsync(int ownerId);
    Task AddAsync(EquipmentItem item);
    Task UpdateAsync(EquipmentItem item);
    Task DeleteAsync(int id);
}

public class EquipmentService(IDbContextFactory<AppDb> factory) : IEquipmentService
{
    public async Task<List<EquipmentItem>> GetAllAsync()
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Items.Include(i => i.Owner).ToListAsync();          // READ
    }
    public async Task<EquipmentItem?> GetByIdAsync(int id)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Items.Include(i => i.Owner).FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task<List<EquipmentItem>> GetByOwnerAsync(int ownerId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Items.Where(i => i.OwnerId == ownerId).ToListAsync();
    }
    public async Task AddAsync(EquipmentItem item)                          // CREATE
    {
        await using var db = await factory.CreateDbContextAsync();
        db.Items.Add(item);
        await db.SaveChangesAsync();
    }
    public async Task UpdateAsync(EquipmentItem item)                       // UPDATE
    {
        await using var db = await factory.CreateDbContextAsync();
        db.Entry(item).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)                                   // DELETE
    {
        await using var db = await factory.CreateDbContextAsync();
        var item = await db.Items.FindAsync(id);
        if (item is null) return;
        db.Items.Remove(item);
        await db.SaveChangesAsync();
    }
}
