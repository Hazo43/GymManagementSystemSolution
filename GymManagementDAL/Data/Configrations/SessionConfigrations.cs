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
    internal class SessionConfigrations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                // Capacity
                Tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25 ");

                // EndDate 
                Tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            builder.HasKey(x => x.Id);
            
            // Category
            builder.HasOne(x => x.SessionCategory)
                   .WithMany(x => x.Sessions)
                   .HasForeignKey(x => x.CategoryId);
          
            // Trainer 
            builder.HasOne( x => x.SessionTrainer)
                   .WithMany(  x => x.TrainerSessions)
                   .HasForeignKey( x => x.TrainerId);

        }
    }
}
