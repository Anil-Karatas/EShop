using EShop.Business.Dtos;
using EShop.Business.Services;
using EShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environmet;
        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environmet)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environmet = environmet;
        }
        public IActionResult List()
        {
            var productDtoList = _productService.GetProducts();
            // Select ile bir tür listeden diğer tür listeye çeviriyorum.
            // Herbir elemanı için yeni bir ListProductViewModel açılıp veriler aktarılıyor.
            var viewModel = productDtoList.Select(x => new ProductListViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                UnitInStock = x.UnitInStock,
                UnitPrice = x.UnitPrice,
                ImagePath = x.ImagePath,
            }).ToList();
            return View(viewModel);
        }
        public IActionResult New()
        {
            ViewBag.Categories = _categoryService.GetCategories();
            return View("Form", new ProductFormViewModel());

        }
        public IActionResult Edit(int id)
        {
            var editProductDto = _productService.GetProductById(id);
            var viewModel = new ProductFormViewModel()
            {
                Id = editProductDto.Id,
                Name = editProductDto.Name,
                Description = editProductDto.Description,
                UnitInStock = editProductDto.UnitInStock,
                UnitPrice = editProductDto.UnitPrice,
                CategoryId = editProductDto.CategoryId,
            };
            // Eski görseli ekranda görmek istiyorum. O yüzden viewBag ile ilgili viewe gönderiyorum.
            ViewBag.ImagePath = editProductDto.ImagePath;
            ViewBag.Categories = _categoryService.GetCategories();
            return View("form", viewModel);
        }
        [HttpPost]
        public IActionResult Save(ProductFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Form", formData);
            }
            var newFileName = "";
            if (formData.File is not null) // Eger bir görsel gönderildiyse
            {
                var allowedFileTypes = new string[] { "image/jpeg", "image/jpg", "image/png", "image/jfif" };  // izin verecegim dosya türleri.
                var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif" }; // izin verecegim dosya uzantıları

                var fileContentType = formData.File.ContentType;  // Dosyanın içerik tipi
                var fileNameWithoutExtesion = Path.GetFileNameWithoutExtension(formData.File.FileName); // uzantısız dosya ismi
                var fileExtension = Path.GetExtension(formData.File.FileName); // Uzantı
                if (!allowedFileTypes.Contains(fileContentType) ||
                    !allowedFileExtensions.Contains(fileExtension))
                {
                    ViewBag.FileError = "Dosya formatı veya içerigi hatalı";
                    ViewBag.Categories = _categoryService.GetCategories();
                    return View("Form", formData);
                }
                newFileName = fileNameWithoutExtesion + "-" + Guid.NewGuid() + fileExtension;
                // aynı isimde iki dosya yüklenildiğinde hat vermesin, birbiriyle asla eşlemeyecek şekilde her dosya adına eşsiz/unique) bir metin ilavesi yapıyorum.
                var folderPath = Path.Combine("images", "products");
                //images/products
                var wwwrootFolderPath = Path.Combine(_environmet.WebRootPath, folderPath);
                //..wwwroot/images/product
                var wwwrootFilePath = Path.Combine(wwwrootFolderPath, newFileName);
                // ...wwwroot/images/products/urungorseli123112211.jpg
                Directory.CreateDirectory(wwwrootFolderPath); // Eğer images/products yoksa oluştur.
                using (var fileStream = new FileStream(
                     wwwrootFilePath, FileMode.Create))
                {
                    formData.File.CopyTo(fileStream);
                }
                //Asıl Dosyayı kopyaladığım kısım.

                // using içerinde new'lenen filestream nesnesi scope boyunca yaşar, scope bitimi silinir.
            }

            if (formData.Id == 0) //Ekleme
            {
                var addProductDto = new AddProductDto()
                {
                    Name = formData.Name.Trim(),
                    Description = formData.Description,
                    UnitInStock = formData.UnitInStock,
                    UnitPrice = formData.UnitPrice,
                    CategoryId = formData.CategoryId,
                    ImagePath=newFileName
                };
                _productService.AddProduct(addProductDto);
            }
            else  //Güncelleme
            {
                var editProductDto = new EditProductDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description,
                    UnitInStock = formData.UnitInStock,
                    UnitPrice = formData.UnitPrice,
                    CategoryId = formData.CategoryId,
                };
                if(formData.File is not null)
                {
                    editProductDto.ImagePath = newFileName;
                }
                _productService.EditProduct(editProductDto);

            }
            return RedirectToAction("List");
        }
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("List");
        }
    }
}
