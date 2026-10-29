using Microsoft.EntityFrameworkCore;
namespace ToolShare.Models;

public class AppDb(DbContextOptions<AppDb> options) : DbContext(options)
{
    public DbSet<Member> Members => Set<Member>();
    public DbSet<EquipmentItem> Items => Set<EquipmentItem>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // avoids SQL Server "multiple cascade paths" error
        mb.Entity<Booking>().HasOne(b => b.Borrower).WithMany()
          .HasForeignKey(b => b.BorrowerId).OnDelete(DeleteBehavior.Restrict);

        // sample data
        mb.Entity<Member>().HasData(
            new Member { Id = 1, Name = "Ali Raza", Email = "ali@mail.com", Location = "Islamabad" },
            new Member { Id = 2, Name = "Sara Khan", Email = "sara@mail.com", Location = "Rawalpindi" });

        mb.Entity<EquipmentItem>().HasData(
            new EquipmentItem { Id = 1, Name = "Cordless Drill (18V)", Category = "Power Tools", Location = "Islamabad", Description = "Includes 2 batteries and charger.", Conditions = "Return fully charged.", OwnerId = 1 },
            new EquipmentItem { Id = 2, Name = "DSLR Camera", Category = "Cameras", Location = "Rawalpindi", Description = "24MP with kit lens.", Conditions = "Handle with care.", OwnerId = 1 },
            new EquipmentItem { Id = 3, Name = "Camping Tent (4 Person)", Category = "Camping Gear", Location = "Islamabad", Description = "Waterproof, easy setup.", OwnerId = 1 },
            new EquipmentItem { Id = 4, Name = "Mountain Bike", Category = "Sports Equipment", Location = "Lahore", Description = "21 gears, good condition.", OwnerId = 2 });
    }
}
