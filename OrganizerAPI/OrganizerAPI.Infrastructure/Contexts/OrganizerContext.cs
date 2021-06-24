﻿using Microsoft.EntityFrameworkCore;
using OrganizerAPI.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Infrastructure.Contexts
{
    public partial class OrganizerContext : DbContext
    {
        public OrganizerContext()
        {

        }

        public OrganizerContext(DbContextOptions<OrganizerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTask>(ConfigureUserTask);
            modelBuilder.Entity<User>(ConfigureUser);
        }
    }
}
