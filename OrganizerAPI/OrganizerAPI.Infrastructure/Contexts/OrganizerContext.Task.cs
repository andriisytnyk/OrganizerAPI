﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizerAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Infrastructure.Contexts
{
    public partial class OrganizerContext
    {
        private const string TaskTableName = "Tasks";

        public DbSet<Models.Models.Task> Tasks { get; set; }

        public void ConfigureTask(EntityTypeBuilder<Models.Models.Task> builder)
        {
            builder.ToTable(TaskTableName);
            builder.HasKey(t => t.Id).IsClustered();
            builder.Property(t => t.Title).HasMaxLength(50).HasDefaultValue("New task").IsRequired();
            builder.Property(t => t.Description).HasMaxLength(200);
            builder.Property(t => t.Status).HasDefaultValue(Status.ToDo).IsRequired();
        }
    }
}