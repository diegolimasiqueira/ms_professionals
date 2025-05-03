using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Infrastructure.Extensions;
using System.Text.Json;
using MSProfessionals.Infrastructure.Context.Configurations;

namespace MSProfessionals.Infrastructure.Context;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<CountryCode> CountryCodes { get; set; } = null!;
    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<Language> Languages { get; set; } = null!;
    public DbSet<ProfessionalAddress> ProfessionalAddresses { get; set; } = null!;
    public DbSet<Professional> Professionals { get; set; } = null!;
    public DbSet<Domain.Entities.TimeZone> TimeZones { get; set; } = null!;
    public DbSet<Profession> Professions { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<ProfessionalProfession> ProfessionalProfessions { get; set; } = null!;
    public DbSet<ProfessionalService> ProfessionalServices { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("shc_professional");

        modelBuilder.Entity<CountryCode>(entity =>
        {
            entity.ToTable("tb_country_codes");
            entity.HasKey(e => e.Id).HasName("PK_tb_country_codes");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(5).IsRequired();
            entity.Property(e => e.CountryName).HasColumnName("country_name").HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("tb_currencies");
            entity.HasKey(e => e.Id).HasName("PK_tb_currencies");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(3).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("tb_languages");
            entity.HasKey(e => e.Id).HasName("PK_tb_languages");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(10).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<ProfessionalAddress>(entity =>
        {
            entity.ToTable("tb_professional_address");
            entity.HasKey(e => e.Id).HasName("PK_tb_professional_address");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalId).HasColumnName("professional_id").IsRequired();
            entity.Property(e => e.StreetAddress).HasColumnName("street_address").HasMaxLength(255).IsRequired();
            entity.Property(e => e.City).HasColumnName("city").HasMaxLength(30).IsRequired();
            entity.Property(e => e.State).HasColumnName("state").HasMaxLength(50).IsRequired();
            entity.Property(e => e.PostalCode).HasColumnName("postalcode").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Latitude).HasColumnName("latitude").IsRequired(false);
            entity.Property(e => e.Longitude).HasColumnName("longitude").IsRequired(false);
            entity.Property(e => e.IsDefault).HasColumnName("is_default").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();
            entity.Property(e => e.CountryId).HasColumnName("country_id").IsRequired();

            entity.HasOne(e => e.Country)
                .WithMany()
                .HasForeignKey(e => e.CountryId)
                .HasConstraintName("FK_tb_professional_address_tb_country_codes_country_id")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Professional)
                .WithMany(p => p.Addresses)
                .HasForeignKey(e => e.ProfessionalId)
                .HasConstraintName("FK_tb_professional_address_tb_professionals_professional_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.CountryId).HasDatabaseName("IX_tb_professional_address_country_id");
            entity.HasIndex(e => e.ProfessionalId).HasDatabaseName("IX_tb_professional_address_professional_id");
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.ToTable("tb_professionals");
            entity.HasKey(e => e.Id).HasName("PK_tb_professionals");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.DocumentId).HasColumnName("document_id").HasMaxLength(50).IsRequired();
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_url").HasMaxLength(500).IsRequired(false);
            entity.Property(e => e.SocialMedia)
                .HasColumnName("social_media")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => v != null ? JsonSerializer.Serialize(v, (JsonSerializerOptions)null) : null,
                    v => v != null ? JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null) : null
                )
                .Metadata.SetValueComparer(new DictionaryComparer<string, string>());
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id").IsRequired();
            entity.Property(e => e.PhoneCountryCodeId).HasColumnName("phone_country_code_id").IsRequired();
            entity.Property(e => e.PreferredLanguageId).HasColumnName("preferred_language_id").IsRequired();
            entity.Property(e => e.TimezoneId).HasColumnName("timezone_id").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();
            entity.Property(e => e.Media)
                .HasColumnName("media")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => v != null ? JsonSerializer.Serialize(v, (JsonSerializerOptions)null) : null,
                    v => v != null ? JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null) : null
                )
                .Metadata.SetValueComparer(new DictionaryComparer<string, string>());

            entity.HasIndex(e => e.DocumentId).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.PhoneNumber).IsUnique();

            entity.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .HasConstraintName("FK_tb_professionals_tb_currencies_currency_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.PhoneCountryCode)
                .WithMany(c => c.Professionals)
                .HasForeignKey(e => e.PhoneCountryCodeId)
                .HasConstraintName("FK_tb_professionals_tb_country_codes_phone_country_code_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.PreferredLanguage)
                .WithMany()
                .HasForeignKey(e => e.PreferredLanguageId)
                .HasConstraintName("FK_tb_professionals_tb_languages_preferred_language_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Timezone)
                .WithMany()
                .HasForeignKey(e => e.TimezoneId)
                .HasConstraintName("FK_tb_professionals_tb_time_zones_timezone_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.PhoneCountryCodeId).HasDatabaseName("IX_tb_professionals_phone_country_code_id");
            entity.HasIndex(e => e.CurrencyId).HasDatabaseName("IX_tb_professionals_currency_id");
            entity.HasIndex(e => e.PreferredLanguageId).HasDatabaseName("IX_tb_professionals_preferred_language_id");
            entity.HasIndex(e => e.TimezoneId).HasDatabaseName("IX_tb_professionals_timezone_id");
        });

        modelBuilder.Entity<Domain.Entities.TimeZone>(entity =>
        {
            entity.ToTable("tb_time_zones");
            entity.HasKey(e => e.Id).HasName("PK_tb_time_zones");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.ToTable("tb_professions");
            entity.HasKey(e => e.Id).HasName("PK_tb_professions");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.Name).HasDatabaseName("UQ_tb_professions_name").IsUnique();
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("tb_services");
            entity.HasKey(e => e.Id).HasName("PK_tb_services");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.Name).HasDatabaseName("UQ_tb_services_name").IsUnique();
        });

        modelBuilder.Entity<ProfessionalProfession>(entity =>
        {
            entity.ToTable("tb_professional_professions");
            entity.HasKey(e => e.Id).HasName("PK_tb_professional_professions");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalId).HasColumnName("professional_id").IsRequired();
            entity.Property(e => e.ProfessionId).HasColumnName("profession_id").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();

            entity.HasOne(e => e.Professional)
                .WithMany(p => p.ProfessionalProfessions)
                .HasForeignKey(e => e.ProfessionalId)
                .HasConstraintName("FK_tb_professional_professions_tb_professionals_professional_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Profession)
                .WithMany(p => p.ProfessionalProfessions)
                .HasForeignKey(e => e.ProfessionId)
                .HasConstraintName("FK_tb_professional_professions_tb_professions_profession_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ProfessionalId).HasDatabaseName("IX_tb_professional_professions_professional_id");
            entity.HasIndex(e => e.ProfessionId).HasDatabaseName("IX_tb_professional_professions_profession_id");
        });

        modelBuilder.Entity<ProfessionalService>(entity =>
        {
            entity.ToTable("tb_professional_services");
            entity.HasKey(e => e.Id).HasName("PK_tb_professional_services");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfessionId).HasColumnName("professional_profession_id").IsRequired();
            entity.Property(e => e.ServiceId).HasColumnName("service_id").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();

            entity.HasOne(e => e.ProfessionalProfession)
                .WithMany(p => p.ProfessionalServices)
                .HasForeignKey(e => e.ProfessionalProfessionId)
                .HasConstraintName("FK_tb_professional_services_tb_professional_professions_professional_profession_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Service)
                .WithMany(s => s.ProfessionalServices)
                .HasForeignKey(e => e.ServiceId)
                .HasConstraintName("FK_tb_professional_services_tb_services_service_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ProfessionalProfessionId).HasDatabaseName("IX_tb_professional_services_professional_profession_id");
            entity.HasIndex(e => e.ServiceId).HasDatabaseName("IX_tb_professional_services_service_id");
        });

        modelBuilder.ApplyConfiguration(new ProfessionalAddressEntityConfiguration());
    }
}
