using EShop.Business.Managers;
using EShop.Business.Services;
using EShop.Data.Context;
using EShop.Data.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//3)) 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EShopContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
//IRepository'e dependecy injenction yap�ld���nda, SqlReposistory nesnesi g�nder.

// businessten sonra buraya geldik.
//AddScoped = Her istek i�in yeni bir kopya
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();
//Authotenciationdan buraya geliyoruz. Cookie tan�mlayaca��z.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");  // Home/Index'e gidecek
    options.LogoutPath = new PathString("/");   // Home/Index'e gidecek
    options.AccessDeniedPath = new PathString("/");  // Home/Index'e gidecek.

    // Giri�-��k��-Eri�im Reddi durumlarda AnaSayfaya y�nlendiriyorum.
});



var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
// �stekiler kimlik belirleme ve yetkilendirme i�lemi i�in gerekli.

//Area i�in buraya geldik. Defaultta herzaman altta olacak.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}"
    );
// �stekinin yerine alttakini yazabiliriz.
// app.MapDefaultControllerRoute();   

app.Run();
