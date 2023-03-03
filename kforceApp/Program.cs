using kforceApp.Contexts;
using kforceApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICarRepository, CarRepository>();

builder.Services.AddDbContext<appContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBconectionSQL"))
);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Car}/{action=Index}/{id?}");

app.Run();

