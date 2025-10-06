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
    internal class HealthRecordConfigration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            // Member بتاع ال Table هيتحول في نفس ال
            builder.ToTable("Members");

            builder.HasKey(x => x.Id); // Not Nedded [ By Convention

            builder.HasOne<Member>()
                   .WithOne(x => x.HealthRecord)
                   .HasForeignKey<HealthRecord>(x => x.Id);

            // => عشان متكرره مرتين Ignore عشان كدا عملتلها member ال table كدا كدا هتبقي موجوده جوا  
            builder.Ignore(x => x.CreatedAt);
            builder.Ignore(x => x.UpdatedAt);

        }
    }
}
