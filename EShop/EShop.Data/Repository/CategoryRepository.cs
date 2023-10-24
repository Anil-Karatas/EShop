//using EShop.Data.Context;
//using EShop.Data.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EShop.Data.Repository
//{
//    public class CategoryRepository : ICatogoryRepository
//    {
//        private readonly EShopContext _db;
//        private readonly DbSet<CategoryEntity> _dbSet;

//        public CategoryRepository(EShopContext db)
//        {
//            _db = db;
//            _dbSet = _db.Set<CategoryEntity>();
//        }

//        public void Add(CategoryEntity entity)
//        {
//            _dbSet.Add(entity);
//            _db.SaveChanges();
//        }

//        public void Delete(int id)
//        {
//            var entity = _dbSet.Find(id);
            
//            entity.IsDeleted=true;
//            entity.ModifiedDate= DateTime.Now;
//            _dbSet.Update(entity);
//            _db.SaveChanges();

            
//        }

//        public CategoryEntity GetById(int id)
//        {
//            return _dbSet.Find(id);
//        }

//        public void Update(CategoryEntity entity)
//        {
//            entity.ModifiedDate = DateTime.Now;
//            _dbSet.Update(entity);
//            _db.SaveChanges();
//        }
//    }
//}
