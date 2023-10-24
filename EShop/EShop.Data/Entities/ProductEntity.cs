using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }

        public bool Favorite { get; set; }   // favori için yaptık
        // 1 ürün 1 kategoriye ait

        //Relational Property

        public CategoryEntity Category { get; set; }
    }
        public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50); // isim zorunlu ve maximum 50 karakter.
            builder.Property(x => x.Description).IsRequired(false); //açıklama zorunlu
            builder.Property(x => x.UnitPrice).IsRequired(false);
            builder.Property(x => x.UnitPrice).IsRequired(false);
            builder.Property(x => x.ImagePath).IsRequired(false);
            builder.Property(x => x.CategoryId).IsRequired();

            base.Configure(builder);
        }
    }
    
}
