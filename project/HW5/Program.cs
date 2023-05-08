using HW5.DAO;
using HW5.DAO.Implementation;
using HW5.Models;
using HW5.Services;
using HW5.Services.Implementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionMethod = (builder.Configuration["PrefferedDataBaseConnectionMethode"] ?? "EF").ToUpper().Trim();
switch (connectionMethod)
{
    case "EF":
        {
            builder.Services.AddTransient<IAnalysisDao, AnalysisEfDao>();
            builder.Services.AddTransient<IOrderDao, OrderEfDao>();
            break;
        }
    case "ADO":
        {
            builder.Services.AddTransient<IAnalysisDao, AnalysisAdoDao>();
            builder.Services.AddTransient<IOrderDao, OrderAdoDao>();
            break;
        }
    default:
        {
            throw new Exception("Invalid preferred database connection method");
        }
}

builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IAnalysisService, AnalysisService>();
builder.Services.AddDbContext<OrdersDBContext>(
        options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}");

app.Run();
