using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Configurations;

/// <summary>
/// Professional entity configuration
/// </summary>
public class ProfessionalEntityConfiguration : IEntityTypeConfiguration<Professional>
{
    /// <summary>
    /// Configures the Professional entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.ToTable("tb_professionals");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);

        builder.HasMany(p => p.Addresses)
            .WithOne(pa => pa.Professional)
            .HasForeignKey(pa => pa.ProfessionalId);

        builder.HasMany(p => p.ProfessionalProfessions)
            .WithOne(pp => pp.Professional)
            .HasForeignKey(pp => pp.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 