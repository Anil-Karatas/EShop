using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        
        TEntity GetById(int id);
        

        TEntity Get(Expression<Func<TEntity, bool>> predicate); //Linq için değişken tipi
        IQueryable<TEntity> GetAll(Expression<Func<TEntity,bool>> predicate=null);
    }
}
