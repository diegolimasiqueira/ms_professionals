using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Configurations;

/// <summary>
/// Configuration for the Profession entity
/// </summary>
public class ProfessionEntityConfiguration : IEntityTypeConfiguration<Profession>
{
    /// <summary>
    /// Configures the Profession entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Profession> builder)
    {
        builder.ToTable("tb_professions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.ProfessionalProfessions)
            .WithOne(pp => pp.Profession)
            .HasForeignKey(pp => pp.ProfessionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 