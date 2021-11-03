using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NC_21.Models;

namespace NC_21.Models
{
    public class NC_21Context : DbContext
    {
        public NC_21Context (DbContextOptions<NC_21Context> options)
            : base(options)
        {
        }

        public DbSet<NC_21.Models.Institut> Institut { get; set; }

        public DbSet<NC_21.Models.Field> Field { get; set; }

        public DbSet<NC_21.Models.Group> Group { get; set; }

        public DbSet<NC_21.Models.Student> Student { get; set; }
    }
}
