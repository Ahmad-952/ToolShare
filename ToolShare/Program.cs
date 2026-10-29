using Microsoft.EntityFrameworkCore;
using ToolShare.Components;
using ToolShare.Models;
using ToolShare.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// database + services (dependency injection)
builder.Services.AddDbContextFactory<AppDb>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<UserState>();

var app = builder.Build();

// creates the database + sample data automatically
using (var scope = app.Services.CreateScope())
{
    var f = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDb>>();
    using var db = f.CreateDbContext();
    db.Database.EnsureCreated();
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
