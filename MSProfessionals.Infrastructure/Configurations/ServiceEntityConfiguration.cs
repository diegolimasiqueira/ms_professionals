using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Configurations;

/// <summary>
/// Service entity configuration
/// </summary>
public class ServiceEntityConfiguration : IEntityTypeConfiguration<Service>
{
    /// <summary>
    /// Configures the Service entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(s => s.ProfessionalServices)
            .WithOne(ps => ps.Service)
            .HasForeignKey(ps => ps.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 