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
//    public class UserRepository : IUserRepository
//    {
//        private readonly EShopContext _db;
//        private readonly DbSet<UserEntity> _dbSet;
//        public UserRepository(EShopContext db)
//        {
//            _db = db;
//            _dbSet=_db.Set<UserEntity>();  //_db.Users yerine _dbSet yazılabilir.
//        }
//        public void Add(UserEntity entity)
//        {
//           _dbSet.Add(entity);
//            _db.SaveChanges();
//        }

//        public void Delete(int id)
//        {
//            var entity=_dbSet.Find(id);
//            entity.IsDeleted=true; 
//            entity.ModifiedDate=DateTime.Now;
//            _dbSet.Update(entity);
//            _db.SaveChanges();

//        }

//        public UserEntity GetById(int id)
//        {
//            return _dbSet.Find(id);
//        }

//        public void Update(UserEntity entity)
//        {
//            entity.ModifiedDate= DateTime.Now;
//            _dbSet.Update(entity);
//            _db.SaveChanges();
//        }
//    }
//}
