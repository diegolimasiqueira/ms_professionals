using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Configurations;

/// <summary>
/// Country entity configuration
/// </summary>
public class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
{
    /// <summary>
    /// Configures the Country entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.HasMany(c => c.ProfessionalAddresses)
            .WithOne(pa => pa.Country)
            .HasForeignKey(pa => pa.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 