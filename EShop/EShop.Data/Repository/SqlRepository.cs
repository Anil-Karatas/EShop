using EShop.Data.Context;
using EShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.Repository
{
    public class SqlRepository<TEntities> : IRepository<TEntities> where TEntities : BaseEntity
    {
        private readonly EShopContext _db;
        private readonly DbSet<TEntities> _dbSet;

        public SqlRepository(EShopContext db)
        {
            _db = db;
            _dbSet=db.Set<TEntities>();

        }
        public void Add(TEntities entity)
        {
            _dbSet.Add(entity);
            _db.SaveChanges();
        }

        

        public void Delete(int id)
        {
            var entity =_dbSet.Find(id);
            Delete(entity);
            
        }

        public void Delete(TEntities entity)
        {
            entity.IsDeleted = true;
            entity.ModifiedDate= DateTime.Now;
            _dbSet.Update(entity);
            _db.SaveChanges();

            
        }

        public TEntities Get(Expression<Func<TEntities, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);

            // First == ilkini yakalar, başka varsa hata vermez. Yoksa hata verir.
            //FirstOrDefault == ilkini yakalar, yoksa hata vermez geriye null döner.
            //Single = ilkini yakalar başka çıkarsa hata verir.
            //SingleOrDefault == ilkini yakalar yoksa null döner başka varsa error verir. yoksa null döner.
        }

        public IQueryable<TEntities> GetAll(Expression<Func<TEntities, bool>> predicate = null)
        {
            return predicate is not null ? _dbSet.Where(predicate) : _dbSet;
        }

        public TEntities GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntities entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
            _db.SaveChanges();
        }

    }
}
