using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configrations
{
    internal class MemberConfigration :  GymUserConfigrations<Member> , IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x => x.CreatedAt)
                   .HasColumnName("JoinDate")  // JoinDate هيبقي اسمو كدا Member جوا ال 
                   .HasDefaultValueSql("GETDATE()"); // دي في حاله لو سابها فاضيه


            // GymUserConfigrations دي بتروح تنفذ كل الي جوا ال
            // دي لازم تكون في الاخر عشان لو كتبنا اي تعديل تحتها مش هيتنفذ
            base.Configure(builder);
        }
    }
}
