using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QuestBoard.Models;
using QuestBoard.Services;

var builder = WebApplication.CreateBuilder(args);

// SQLite; suppress PendingModelChanges warning for dev
var cs = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=QuestBoard.db";
builder.Services.AddDbContext<QuestBoardContext>(opt =>
{
    opt.UseSqlite(cs);
    opt.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddControllersWithViews();

// ✅ DI registrations
builder.Services.AddScoped<IQuestService, QuestService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

var app = builder.Build();

// Create schema and seed (no migrations required for dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuestBoardContext>();
    await db.Database.EnsureCreatedAsync();
    await SeedData.InitializeAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quests}/{action=Index}/{id?}");

app.Run();
