using EShop.Data.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.Entities
{
    public class UserEntity:BaseEntity
    {
        public string EMail { get; set; }
        public string Password { get; set; }
        public string FistName { get; set; }
        public string  LastName { get; set; }
        public UserTypeEnum UserType { get; set; }

    }
    public class UserConfiguration:BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FistName)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(x=>x.LastName)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(x => x.EMail)
                .IsRequired().HasMaxLength(30);
            builder.Property(x => x.Password)
                .IsRequired();
            base.Configure(builder);
        }
    }
}
