﻿using HRapi.Models;
using Microsoft.EntityFrameworkCore;

namespace HRapi.Data
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
