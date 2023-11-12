using CardsLand_Web.Implementations;
using CardsLand_Web.Interfaces;
using CardsLand_Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddSingleton<IUserModel, UserModel>();
builder.Services.AddSingleton<ITools, Tools>();
builder.Services.AddSingleton<IPokemonTcgModel, PokemonTcgModel>();



builder.Services.AddSingleton<IUserModel, UserModel>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
-----------------------------------------------------------------------------------