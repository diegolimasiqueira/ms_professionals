using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Configurations;

/// <summary>
/// Professional profession entity configuration
/// </summary>
public class ProfessionalProfessionEntityConfiguration : IEntityTypeConfiguration<ProfessionalProfession>
{
    /// <summary>
    /// Configures the ProfessionalProfession entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<ProfessionalProfession> builder)
    {
        builder.ToTable("ProfessionalProfessions");

        builder.HasKey(pp => pp.Id);

        builder.Property(pp => pp.ProfessionalId)
            .IsRequired();

        builder.Property(pp => pp.ProfessionId)
            .IsRequired();

        builder.Property(pp => pp.IsMain)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(pp => pp.CreatedAt)
            .IsRequired();

        builder.Property(pp => pp.UpdatedAt)
            .IsRequired(false);

        builder.HasOne(pp => pp.Professional)
            .WithMany(p => p.ProfessionalProfessions)
            .HasForeignKey(pp => pp.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pp => pp.Profession)
            .WithMany()
            .HasForeignKey(pp => pp.ProfessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(pp => new { pp.ProfessionalId, pp.ProfessionId })
            .IsUnique();

        builder.HasIndex(pp => new { pp.ProfessionalId, pp.IsMain })
            .IsUnique()
            .HasFilter("[IsMain] = 1");
    }
} 