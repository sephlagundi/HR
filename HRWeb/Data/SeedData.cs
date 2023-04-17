﻿using HRWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HRWeb.Data
{
    public static class SeedData
    {
        public static void SeedDefaultData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
            new Employee(1, "John Padua", new DateTime(1998, 12, 1), "lexter.padua17@gmail.com", "09453823795", 1, "very bad noob."),
            new Employee(2, "Din Djarin", new DateTime(1995, 2, 3), "mando@email.com", "09453823796", 1, "Completes given task."),
            new Employee(3, "Arthur Morgan", new DateTime(1993, 5, 12), "arthur@email.com", "09453823797", 2, "Loyal and follow rules."),
            new Employee(4, "Frodo Baggins", new DateTime(1992, 11, 15), "frodo@email.com", "09453823798", 3, "Loyal cannot be corrupted."));



            modelBuilder.Entity<Department>().HasData(
                new Department(1, "IT Department"),
                new Department(2, "Accounting Department"),
                new Department(3, "Human Resources Department"),
                new Department(4, "Marketing Department"));

            modelBuilder.Entity<LeaveType>().HasData(
                new Department(1, "Birthday Leave"),
                new Department(2, "Vacation Leave"),
                new Department(3, "Emergency Leave"),
                new Department(4, "Leave Without Pay"),
                new Department(5, "Magna Carta Leave"),
                new Department(6, "Sick Leave"),
                new Department(7, "Official Business"),
                new Department(8, "Solo Parent Leave"));
        }
    }
}
