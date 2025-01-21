using AutorizationAndCRUD.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(option => option.UseSqlServer(connection));
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60); //для наглядности, поставим время сессии 60 секунд
});
var app = builder.Build();
app.UseSession();

app.MapStaticAssets();
app.MapDefaultControllerRoute();

app.Run();
