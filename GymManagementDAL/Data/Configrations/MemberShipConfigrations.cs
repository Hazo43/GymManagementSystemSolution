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
    internal class MemberShipConfigrations : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(x => x.CreatedAt)
                   .HasColumnName("StartDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => new { x.PlanId, x.MemberId });

            builder.Ignore(x => x.Id);  // Table ولا  PK عشان ميروحش يعملو

            // Plan 
            builder.HasOne(x => x.Plan)
                   .WithMany(x => x.PlanMembers)
                   .HasForeignKey(x => x.PlanId);

            // Member 
            builder.HasOne(x => x.Member)
                   .WithMany(x => x.MemberShips)
                   .HasForeignKey(x => x.MemberId);
        }
    }
}
