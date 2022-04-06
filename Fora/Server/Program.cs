global using Fora.Server.Data;
global using Fora.Server.Models;
global using Fora.Shared;
global using Fora.Shared.DTO;
global using Microsoft.AspNetCore.Identity;
global using Fora.Server.Models;
global using Microsoft.EntityFrameworkCore;
using Fora.Server.Services.InterestService;
using Fora.Server.Services.UserService;
using Fora.Server.Services.AccountService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var authConnectionString = builder.Configuration.GetConnectionString("AuthConnection");


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(authConnectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

// skapa admin konto
using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
{
    var context = serviceProvider.GetRequiredService<AuthDbContext>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    context.Database.Migrate();

    if (!context.Users.Any())
    {
        // Skapa roll
        IdentityRole adminRole = new();
        adminRole.Name = "Admin";

        await roleManager.CreateAsync(adminRole);

        // Skapa användare
        ApplicationUser newUser = new();
        newUser.UserName = "admin";
        newUser.Email = "anders@admin.se";
        string password = "Admin1#";

        await userManager.CreateAsync(newUser, password);

        // Tilldela roll
        await userManager.AddToRoleAsync(newUser, "Admin");
    }
}

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IInterestService, InterestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
