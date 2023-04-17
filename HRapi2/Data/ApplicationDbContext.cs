using HRapi2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HRapi2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        //public DbSet<Department> departments { get; set; }
        //public DbSet<Benefit> benefits { get; set; }

    }
}
