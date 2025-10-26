using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<QuestBoardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// 3️⃣ Build the app AFTER services are registered
var app = builder.Build();

// 4️⃣ Optional logging
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Environment: {env}", app.Environment.EnvironmentName);

// 5️⃣ Developer error page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 6️⃣ MVC route mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
