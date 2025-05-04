using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Configurations;

/// <summary>
/// Professional service entity configuration
/// </summary>
public class ProfessionalServiceEntityConfiguration : IEntityTypeConfiguration<ProfessionalService>
{
    /// <summary>
    /// Configures the ProfessionalService entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<ProfessionalService> builder)
    {
        builder.ToTable("tb_professional_services");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.ProfessionalProfessionId)
            .IsRequired();

        builder.Property(ps => ps.ServiceId)
            .IsRequired();

        builder.Property(ps => ps.CreatedAt)
            .IsRequired();

        builder.Property(ps => ps.UpdatedAt)
            .IsRequired(false);

        builder.HasOne(ps => ps.ProfessionalProfession)
            .WithMany(pp => pp.ProfessionalServices)
            .HasForeignKey(ps => ps.ProfessionalProfessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.Service)
            .WithMany()
            .HasForeignKey(ps => ps.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add unique constraint to prevent duplicate services for the same professional profession
        builder.HasIndex(ps => new { ps.ProfessionalProfessionId, ps.ServiceId })
            .IsUnique()
            .HasDatabaseName("UQ_tb_professional_services_professional_profession_id_service_id");
    }
} 