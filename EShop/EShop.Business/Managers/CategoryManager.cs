using EShop.Business.Dtos;
using EShop.Business.Services;
using EShop.Data.Entities;
using EShop.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Business.Managers
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;
        public CategoryManager(IRepository<CategoryEntity> categoryRepository)
        {
            _categoryRepository= categoryRepository;
        }
        public bool AddCategory(AddCategoryDto addCategoryDto)
        {
            var hasCategory= _categoryRepository.GetAll(x=>x.Name.ToLower()==addCategoryDto.Name.ToLower()).ToList();
            if(hasCategory.Any()) //hasCategory !=0 demek
            {
                return false;
            }
            var categoryEntity = new CategoryEntity()
            {
                Name = addCategoryDto.Name,
                Description = addCategoryDto.Description,
            };
            _categoryRepository.Add(categoryEntity);
            return true;

        }

        public void DeleteCategory(int id)
        {
           _categoryRepository.Delete(id);
        }

        public void EditCategory(EditCategoryDto editCategoryDto)
        {
            var categoryEntity = _categoryRepository.GetById(editCategoryDto.Id);

            categoryEntity.Name = editCategoryDto.Name;
            categoryEntity.Description = editCategoryDto.Description;

            _categoryRepository.Update(categoryEntity);
        }

        public List<ListCategoryDto> GetCategories()
        {
           var categoryEntites=_categoryRepository.GetAll().OrderBy(x=>x.Name).ToList();
           var categoryDtoList=categoryEntites.Select(x=>new ListCategoryDto
           {
               Id= x.Id,
               Name= x.Name,
               Description= x.Description,
           }).ToList();
            //Her bir veri için (x) 1 adet ListCategoryDto nesnesi new'leyip, verileri aktarıyorum.
            // Bir tür listeden diğer tür listeye çeviriyorum.
            return categoryDtoList;
        }

        public EditCategoryDto GetCategoryById(int id)
        {
            var categoryEntity = _categoryRepository.GetById(id);

            var editCategoryDto = new EditCategoryDto()
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                Description = categoryEntity.Description
            };

            return editCategoryDto;
        }
        
    }
}
