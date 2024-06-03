using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure
{
    public class CleanArchitectureDbContext : IdentityDbContext
    {
        public CleanArchitectureDbContext(DbContextOptions<CleanArchitectureDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanArchitectureDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
