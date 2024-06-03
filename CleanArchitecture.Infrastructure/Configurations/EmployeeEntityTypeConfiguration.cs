using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations
{
    public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.Name).HasMaxLength(30);

            builder.Property(e => e.Email).HasMaxLength(30);

            builder.Property(e=>e.Guid).HasDefaultValueSql("NEWID()");
        }
    }
}
