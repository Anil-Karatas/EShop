using EShop.Business.Dtos;
using EShop.Business.Services;
using EShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult List()
        {
            var listCategoryDtos = _categoryService.GetCategories();

            var viewModel = listCategoryDtos.Select(x => new CategoryListViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description is not null && x.Description.Length > 20 ? x.Description.Substring(0, 20) + "..." : x.Description
            }).ToList();

            return View(viewModel);
            
        }

        [HttpGet]
        public  IActionResult New() // burada Ekleme ve güncellemeyi aynı anda yapacağız.
        {
            //form üzerindeki bir tasarımı id değerine göre farklılaştıracaksanız, ekleme sayfasını boş bile olsa bir modelle açmamız gerekir.
            return View("Form", new CategoryFormViewModel());
        }
        public IActionResult Edit(int id)
        {
            var editCategoryDto=_categoryService.GetCategoryById(id);
            var viewModel = new CategoryFormViewModel()
            {
                Id = editCategoryDto.Id,
                Name = editCategoryDto.Name,
                Description = editCategoryDto.Description
            };
            return View("form", viewModel);
        }
        [HttpPost]
        public IActionResult Save(CategoryFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", formData);
            }
            
            if(formData.Id==0) // Yeni Kayıt
            {
                var addCategoryDto = new AddCategoryDto()
                {
                    Name = formData.Name.Trim(),
                    //Description = formData.Description.Trim()  // eğer Description null olursa trim işlemi sırasında uygulama exception verir.O nedenle trim yapmak istiyorsak aşağıdaki kontrolü yapmalıyız.
                };
              
                if(formData.Description is not null)
                {
                    addCategoryDto.Description = formData.Description.Trim();
                }
                var result = _categoryService.AddCategory(addCategoryDto);
                if (result)
                {
                    RedirectToAction("List");
                }
                else
                {
                    ViewBag.ErrorMessage = "Bu isimde kategori mevcut";
                    return View("Form", formData);
                }
            }
            else  //Kayıt Güncelleme
            {
                var editCategoryDto = new EditCategoryDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description
                };
                _categoryService.EditCategory(editCategoryDto);
                return RedirectToAction("List");

            }
            return RedirectToAction("List");
        }
        public IActionResult Delete(int id)
        {
            _categoryService.DeleteCategory(id);
            return RedirectToAction("List");
        }
    }
    
}
