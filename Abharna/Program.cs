using Abharna.Helpers;
using Abharna.Models;
using Abharna.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { FullName = "Faisal Al-Harbi", Email = "staff@abharna.sa", Phone = "0500000001", Password = PasswordHelper.Hash("Staff.port"), AccountType = "Port Staff" },
            new User { FullName = "Sara Al-Otaibi", Email = "manager@abharna.sa", Phone = "0500000002", Password = PasswordHelper.Hash("Manager.port"), AccountType = "Port Manager" },
            new User { FullName = "Layla Al-Ghamdi", Email = "customer@abharna.sa", Phone = "0500000003", Password = PasswordHelper.Hash("Customer.port"), AccountType = "Customer" }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
