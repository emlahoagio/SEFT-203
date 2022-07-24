using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepoContext : IdentityDbContext<User> // New for JWT
    {
        public RepoContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // New for JWT

            builder.ApplyConfiguration(new CompanyConfig());
            builder.ApplyConfiguration(new EmployeeConfig());
            builder.ApplyConfiguration(new RoleConfiguration());
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}

