using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectSmoothboard23.Models;

namespace ProjectSmoothboard23.Models
{
    public class SmoothboardDBContext : DbContext
    {
        public SmoothboardDBContext (DbContextOptions<SmoothboardDBContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectSmoothboard23.Models.Product> Product { get; set; }

        public DbSet<ProjectSmoothboard23.Models.FAQ> FAQ { get; set; }

        public DbSet<ProjectSmoothboard23.Models.Subscription> Subscription { get; set; }
    }
}
