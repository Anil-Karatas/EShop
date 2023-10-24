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
//IRepository'e dependecy injenction yapýldýðýnda, SqlReposistory nesnesi gönder.

// businessten sonra buraya geldik.
//AddScoped = Her istek için yeni bir kopya
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();
//Authotenciationdan buraya geliyoruz. Cookie tanýmlayacaðýz.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");  // Home/Index'e gidecek
    options.LogoutPath = new PathString("/");   // Home/Index'e gidecek
    options.AccessDeniedPath = new PathString("/");  // Home/Index'e gidecek.

    // Giriþ-Çýkýþ-Eriþim Reddi durumlarda AnaSayfaya yönlendiriyorum.
});



var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
// Üstekiler kimlik belirleme ve yetkilendirme iþlemi için gerekli.

//Area için buraya geldik. Defaultta herzaman altta olacak.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}"
    );
// üstekinin yerine alttakini yazabiliriz.
// app.MapDefaultControllerRoute();   

app.Run();
