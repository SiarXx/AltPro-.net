using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace AltPro.BackTracker.Models
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {

        }
        public DbSet<TaskModel> TaskModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
