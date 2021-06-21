using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizerAPI.Models.Common;
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
        private const string UsersTableName = "Users";

        public DbSet<User> Users { get; set; }

        public void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(UsersTableName);
            builder.HasKey(u => u.Id).IsClustered();
            builder.Property(u => u.FirstName).HasMaxLength(50);
            builder.Property(u => u.LastName).HasMaxLength(50);
            builder.Property(u => u.Username).HasMaxLength(20).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(30).IsRequired();
            builder.HasMany(u => u.UserRefreshTokens)
                .WithOne()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.UserTasks)
                .WithOne()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
