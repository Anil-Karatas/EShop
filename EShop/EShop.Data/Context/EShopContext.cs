using EShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.Context
{
    public class EShopContext : DbContext

    {

        // 1)

        public EShopContext(DbContextOptions<EShopContext> options) : base(options)
        {
            //2 için app.develop'a git.
        }

        //FluentAPI -> C# tarafındaki clasların sql tablolarına dönüştürülürken özelliklerine yapılan biçimlerdir.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> Users /* { get; set; } */ =>Set<UserEntity>();
        public DbSet<ProductEntity>Products=>Set<ProductEntity>();
        public DbSet<CategoryEntity> Categories=>Set<CategoryEntity>();

    }
}
