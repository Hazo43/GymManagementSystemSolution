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
    internal class TrainerConfigrations : GymUserConfigrations<Trainer> , IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(x => x.CreatedAt)
                  .HasColumnName("HireDate")  // HireDate هيبقي اسمو كدا Trainer جوا ال 
                  .HasDefaultValueSql("GETDATE()"); // دي في حاله لو سابها فاضيه



            // GymUserConfigrations دي بتروح تنفذ كل الي جوا ال
            // دي لازم تكون في الاخر عشان لو كتبنا اي تعديل تحتها مش هيتنفذ
            base.Configure(builder);
       
        }
    }
}
