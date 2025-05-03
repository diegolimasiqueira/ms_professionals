using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Configurations;

/// <summary>
/// Professional address entity configuration
/// </summary>
public class ProfessionalAddressEntityConfiguration : IEntityTypeConfiguration<ProfessionalAddress>
{
    /// <summary>
    /// Configures the ProfessionalAddress entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<ProfessionalAddress> builder)
    {
        builder.ToTable("tb_professional_address");

        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.ProfessionalId)
            .IsRequired();

        builder.Property(pa => pa.StreetAddress)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pa => pa.City)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(pa => pa.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pa => pa.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(pa => pa.CountryId)
            .IsRequired();

        builder.Property(pa => pa.Latitude)
            .IsRequired(false);

        builder.Property(pa => pa.Longitude)
            .IsRequired(false);

        builder.Property(pa => pa.IsDefault)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(pa => pa.CreatedAt)
            .IsRequired();

        builder.Property(pa => pa.UpdatedAt)
            .IsRequired(false);

        builder.HasOne(pa => pa.Professional)
            .WithMany(p => p.Addresses)
            .HasForeignKey(pa => pa.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pa => pa.Country)
            .WithMany()
            .HasForeignKey(pa => pa.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(pa => new { pa.ProfessionalId, pa.IsDefault })
            .IsUnique()
            .HasFilter("[IsDefault] = 1");
    }
} 