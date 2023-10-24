using EShop.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]  //Program.cs'teki area:exists kısmı ile eşleşir.
    [Authorize(Roles ="Admin")] // Claimlerdeki ClaimTypes.Role şeklinde tutulan yetki ile bağlantılı (login action)

    // yukarıda tanımladıgım Authorize saayesinde, yetkisi admin olmayan kişiler buraya istek atmaya çalıştıgında accessDenied veriyoruz ve istedigimiz yere aktarıyoruz. (Program.cs kısmında
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;


        public DashboardController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            ViewBag.userCount = _userService.GetUsers().Count;
            ViewBag.productCount= _productService.GetProducts().Count;

            var toplam = 0;
            foreach(var item in _productService.GetProducts())
            {
               toplam = toplam + Convert.ToInt32(item.UnitPrice*item.UnitInStock);
            }
            ViewBag.toplam = toplam;
            return View();
        }
    }
}
