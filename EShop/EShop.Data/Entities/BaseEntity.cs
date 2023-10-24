using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
    // Base Configuration
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        // Where TEntity:BaseEntity diyerek bu classın yalnızca BaseEntity tipindeki yapılar ile kullanabiliriz.
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasQueryFilter(x => x.IsDeleted == false);
            //Veri tabanı üzerinde ki yapılacak bütün sorgularda yukarıdaki LinQ geçerli olacak. 
            // Böylelikle benim silinmemişleri getir diye yazmama gerek kalmayacak
            builder.Property(x=>x.ModifiedDate).IsRequired(false); 
            // Düzenleme tarihi boş olabilir.
        }
    }

}