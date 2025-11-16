using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
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

// Register health checks, including EF Core database dependency
builder.Services.AddHealthChecks()
    .AddDbContextCheck<QuestBoardContext>(
        name: "database",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "ready", "db" });


// ✅ DI registrations
builder.Services.AddScoped<IQuestService, QuestService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

var app = builder.Build();


app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var payload = new
        {
            status = report.Status.ToString(),
            totalDurationMs = (int)report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                durationMs = (int)e.Value.Duration.TotalMilliseconds
            })
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
    }
});

// Create schema and seed
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
