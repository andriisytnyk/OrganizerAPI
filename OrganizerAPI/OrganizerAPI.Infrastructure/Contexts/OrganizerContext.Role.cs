using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizerAPI.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Infrastructure.Contexts
{
    public partial class OrganizerContext
    {
        private const string RolesTableName = "Roles";

        public DbSet<Role> Roles { get; set; }

        public void ConfigureRole(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(RolesTableName);
            builder.HasKey(r => r.Id).IsClustered();
            builder.Property(r => r.Name).HasMaxLength(20).IsRequired();
            builder.HasMany(r => r.Users)
                .WithOne()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
