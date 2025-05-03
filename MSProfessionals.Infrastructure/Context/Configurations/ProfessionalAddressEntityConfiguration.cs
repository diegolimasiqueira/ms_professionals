using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Context.Configurations;

public class ProfessionalAddressEntityConfiguration : IEntityTypeConfiguration<ProfessionalAddress>
{
    public void Configure(EntityTypeBuilder<ProfessionalAddress> builder)
    {
        builder.ToTable("tb_professional_address", "shc_professional");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.ProfessionalId).HasColumnName("professional_id").IsRequired();
        builder.Property(e => e.StreetAddress).HasColumnName("street_address").HasMaxLength(255).IsRequired();
        builder.Property(e => e.City).HasColumnName("city").HasMaxLength(30).IsRequired();
        builder.Property(e => e.State).HasColumnName("state").HasMaxLength(50).IsRequired();
        builder.Property(e => e.PostalCode).HasColumnName("postalcode").HasMaxLength(20).IsRequired();
        builder.Property(e => e.Latitude).HasColumnName("latitude");
        builder.Property(e => e.Longitude).HasColumnName("longitude");
        builder.Property(e => e.IsDefault).HasColumnName("is_default").IsRequired();
        builder.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(e => e.CountryId).HasColumnName("country_id").IsRequired();

        builder.HasOne(e => e.Professional)
            .WithMany(p => p.Addresses)
            .HasForeignKey(e => e.ProfessionalId)
            .HasConstraintName("FK_tb_professional_address_professional_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Country)
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .HasConstraintName("FK_tb_professional_address_country_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
} 