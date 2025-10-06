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

    // GymUserBaseConfigrations   ممكن نسميها كدا بردو صح
    internal class GymUserConfigrations<T> : IEntityTypeConfiguration<T> where T : GymUserv
    {
        //  عندي اصلا  table عشان هو مش  GymUser هيه مش معموله لل
        // وارثين منها trainer و  member  عشان ال GymUser لل  Configrations احنا عملنا 
      
        public void Configure(EntityTypeBuilder<T> builder)
        {
            // Name
            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            // Email
            builder.Property(x => x.Email)
                  .HasColumnType("varchar")
                  .HasMaxLength(100);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidEmailCheck", "Email Like '_%@_%._%'");
            });
            builder.HasIndex(x => x.Email).IsUnique(); // Unique

            // Phone 
            builder.Property(x => x.Phone)
                  .HasColumnType("varchar")
                  .HasMaxLength(11);
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidPhoneCheck", "Phone Like '01%' and Phone Not Like '%[^0-9]%' ");
            });
            builder.HasIndex(x => x.Phone).IsUnique(); // Unique

            // Address 
            builder.OwnsOne(x => x.Address, AdressBuilder =>
            {
                // Street 
                AdressBuilder.Property(x => x.Street)
                             .HasColumnName("Street")    // Street => DB هيبقي اسمها في ال
                             .HasColumnType("varchar")
                             .HasMaxLength(30);

                // City 
                AdressBuilder.Property(x => x.City)
                            .HasColumnName("City")    // City => DB هيبقي اسمها في ال
                            .HasColumnType("varchar")
                            .HasMaxLength(30);

                // BuildingNumber
                AdressBuilder.Property(x => x.BuildingNumber)
                             .HasColumnName("BuildingNumber");

            });

        }
    
    }
}
