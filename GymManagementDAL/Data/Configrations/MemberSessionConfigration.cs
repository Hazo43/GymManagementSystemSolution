﻿using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configrations
{
    internal class MemberSessionConfigration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        { 
            builder.Ignore( x => x.Id );

            builder.HasKey(x => new { x.MemberId, x.SessionId });

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");

            // Session 
            builder.HasOne(x => x.Session)
                   .WithMany(x => x.SessionMembers)
                   .HasForeignKey(x => x.SessionId);

            // Member 
            builder.HasOne(x => x.Member)
                   .WithMany(x => x.memberSessions)
                   .HasForeignKey(x => x.MemberId);


        }
    }
}
