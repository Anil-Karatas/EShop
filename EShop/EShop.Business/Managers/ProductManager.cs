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
    public class ProductManager : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<CategoryEntity> _categoryRepository;
        public ProductManager (IRepository<ProductEntity> productRepository, IRepository<CategoryEntity> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public bool AddProduct(AddProductDto addProductDto)
        {
            var hasProduct=_productRepository.GetAll(x=>x.Name.ToLower()==addProductDto.Name.ToLower()).ToList();
            if (hasProduct.Any())
            {
                return false;
            }
            var productEntity = new ProductEntity()
            {
                Name = addProductDto.Name,
                Description = addProductDto.Description,
                UnitPrice = addProductDto.UnitPrice,
                UnitsInStock = addProductDto.UnitInStock,
                CategoryId = addProductDto.CategoryId,
                ImagePath= addProductDto.ImagePath
            };
            _productRepository.Add(productEntity);
            return true;
        }

       

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }

        public void EditProduct(EditProductDto editProductDto)
        {
            var productEntity = _productRepository.GetById(editProductDto.Id);
            // Id ile eşleşen nesnesin tamamını yakaladım.
            productEntity.Name= editProductDto.Name;
            productEntity.Description= editProductDto.Description;
            productEntity.UnitPrice= editProductDto.UnitPrice;
            productEntity.UnitsInStock = editProductDto.UnitInStock;
            productEntity.CategoryId= editProductDto.CategoryId;

            if(editProductDto.ImagePath is not null)
            {
                productEntity.ImagePath= editProductDto.ImagePath;
            }
            // bununla birlikte editDtoProduct ile gelen ImagePath bilgisi veritabanındaki görsel bilgisinin üstüne atanmasın. Elimdeki görseli kaybetmeyim.
            _productRepository.Update(productEntity);
        }

        public List<ListProductDto> GetProductByCategoryId(int? categoryId)
        {
           if(categoryId.HasValue)
            {
                var productEntities = _productRepository.GetAll(x => x.CategoryId == categoryId).OrderBy(x => x.Name);
                var productDtos = productEntities.Select(x => new ListProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UnitInStock = x.UnitsInStock,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    ImagePath = x.ImagePath

                }).ToList();
                return productDtos;
            }
            return GetProducts();
        }

        public EditProductDto GetProductById(int id)
        {
            var productEntity= _productRepository.GetById(id);
            var editProductDto = new EditProductDto()
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                UnitPrice = productEntity.UnitPrice,
                UnitInStock = productEntity.UnitsInStock,
                CategoryId = productEntity.CategoryId,
                ImagePath = productEntity.ImagePath,
            };
            return editProductDto;
           
        }

        public ProductDetailDto GetProductDetailById(int id)
        {
            var productEntity = _productRepository.GetById(id);

            var productDetailDto = new ProductDetailDto()
            {
                ProductId = productEntity.Id,
                ProductName = productEntity.Name,
                Description = productEntity.Description,
                UnitInStock = productEntity.UnitsInStock,
                UnitPrice = productEntity.UnitPrice,
                ImagePath = productEntity.ImagePath,
                CategoryId = productEntity.CategoryId,
                CategoryName = _categoryRepository.GetById(productEntity.CategoryId).Name
            };
            return productDetailDto;
        }

        public List<ListProductDto> GetProducts()
        {
            var productEntites = _productRepository.GetAll().OrderBy(x => x.Category.Name).ThenBy(x => x.Name);
            

            var productDtoList = productEntites.Select(x => new ListProductDto
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitsInStock,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                ImagePath=x.ImagePath
            }).ToList();

            return productDtoList;
        }
    }
}
