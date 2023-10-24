using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.Entities
{
    public class CategoryEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }


        // 1 kategoride 1'den fazla ürün bulunabilir.

        //Relational Property

        public List<ProductEntity> Category { get;}
    }
    public class CategoryConfiguration:BaseConfiguration<CategoryEntity>
    {
        // BaseEntity'deki class virtual olduğu için burada override metodu ile eziyoruz.
        public override void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(40);  // isim gerekli ve maximum 40 karakter.
            builder.Property(x => x.Description)
                .IsRequired(false);       // Açıklama gerekli değil.
            base.Configure(builder);
        }
    }
}
