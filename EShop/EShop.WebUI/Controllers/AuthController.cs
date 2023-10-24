using EShop.Business.Dtos;
using EShop.Business.Services;
using EShop.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.WebUI.Controllers
{

    //Authentication * Authorization
    //Kimlik Doğrulama - Yetkilendirme
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel formData)
        {
            if (!ModelState.IsValid) // model istedigim şartlarda değilse aynı sayfaya dön.
            {
                return View(formData); // Yeniden açılınca form girilmiş olan veriler kaybolmasın.
            }
            var addUserDto = new AddUserDto()
            {
                FirstName = formData.FirstName.Trim(),
                LastName = formData.LastName.Trim(),
                EMail = formData.EMail.Trim().ToLower(),
                Password = formData.Password
            };
            var result = _userService.AddUser(addUserDto);

            //eğer herşey yolunda ise beni anasayfaya gönder.
            if (result.IsSucceed)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View(formData);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel formData)
        //bir formdan veri çekiyorsan ilk iş validacation işlemi yapmak
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            //Veriler istediğimiz formatta gelmediyse ana sayfaya geri döner.
            var loginUserDto = new LoginUserDto()
            {
                Email = formData.EMail,
                Password = formData.Password,
            };
            var userInfo=_userService.LoginUser(loginUserDto);
            if(userInfo is null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Buraya kadar kodlar geldi ise EMail ve Şifre eşleşmiş. gerekli bilgiler UserInfo içerisinde (ıd,email,fname,lastname, usertype) veritabanından çekilip gelmiş.

            //Oturumda tutacagım her veri ->Claim
            //Claimlerin listesi ->Claims

            var claims = new List<Claim>();
            claims.Add(new Claim("id",userInfo.Id.ToString()));
            claims.Add(new Claim("email", userInfo.EMail));
            claims.Add(new Claim("firstName",userInfo.FirstName));
            claims.Add(new Claim("lastName",userInfo.LastName));
            claims.Add(new Claim("userType",userInfo.UserType.ToString()));

            //Yetkilendirme işlemleri için özel olarak bir claim açmam gerekiyor.
            claims.Add(new Claim(ClaimTypes.Role, userInfo.UserType.ToString())); //.net metotlarının kullanılacağı söylüyor.
            var claimIndentity=new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //Claims içerisindeki verilerle bir oturum açılacagının söylüyor.

            var autProperties = new AuthenticationProperties
            {
                AllowRefresh = true, //yenilenebilir oturum
                ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(24))   /*TODO : bu kod varken oturum neden açılmıyor kontrol et.*/
                

            };

            // Oturumun özelliklerini belirliyorum.
            //program.cs'e git 
            

            //Asenkronize (async) bir metot kullanıyorsak, await keywordu ile kullanıyoruz.

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIndentity), autProperties);
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // Oturumu Kapat.
            return RedirectToAction("Index", "Home");
        }
    }
}
