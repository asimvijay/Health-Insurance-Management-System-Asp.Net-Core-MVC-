using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Department{ get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<SystemCode> SystemCode { get; set; }

        public DbSet<SystemCodeDetail> SystemCodeDetail { get; set; }
        public DbSet<LeavesType> LeavesType { get; set; }
        public DbSet<Country> Country { get; set; }

        public DbSet<City> City { get; set; }

        public DbSet<LeaveApplication> leaveApplications { get; set; }


    }
}
