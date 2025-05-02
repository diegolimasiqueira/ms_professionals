using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Infrastructure.Data.Configurations;

namespace MSProfessionals.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Professional> Professionals { get; set; }
    public virtual DbSet<CountryCode> CountryCodes { get; set; }
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<Domain.Entities.TimeZone> TimeZones { get; set; }
    public virtual DbSet<ProfessionalAddress> ProfessionalAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.ToTable("tb_professionals", "shc_professional");
            entity.HasKey(e => e.Id);
            
            // Configurações de colunas com tamanho máximo e nulidade
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.DocumentId).HasColumnName("document_id").HasMaxLength(50).IsRequired();
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_url").HasMaxLength(500).IsRequired(false);
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id").IsRequired();
            entity.Property(e => e.PhoneCountryCodeId).HasColumnName("phone_country_code_id").IsRequired();
            entity.Property(e => e.PreferredLanguageId).HasColumnName("preferred_language_id").IsRequired();
            entity.Property(e => e.TimezoneId).HasColumnName("timezone_id").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();

            // Índices únicos
            entity.HasIndex(e => e.DocumentId).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.PhoneNumber).IsUnique();
        });

        modelBuilder.Entity<CountryCode>(entity =>
        {
            entity.ToTable("tb_country_codes", "shc_consumer");
            entity.HasKey(e => e.Id);
            
            // Configurações de colunas com tamanho máximo e nulidade
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(5).IsRequired();
            entity.Property(e => e.CountryName).HasColumnName("country_name").HasMaxLength(100).IsRequired();

            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("tb_currencies", "shc_consumer");
            entity.HasKey(e => e.Id);
            
            // Configurações de colunas com tamanho máximo e nulidade
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(3).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(100).IsRequired();

            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("tb_languages", "shc_consumer");
            entity.HasKey(e => e.Id);
            
            // Configurações de colunas com tamanho máximo e nulidade
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(10).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(100).IsRequired();

            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<Domain.Entities.TimeZone>(entity =>
        {
            entity.ToTable("tb_time_zones", "shc_consumer");
            entity.HasKey(e => e.Id);
            
            // Configurações de colunas com tamanho máximo e nulidade
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(100).IsRequired();

            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.ApplyConfiguration(new ProfessionalAddressEntityConfiguration());
    }
} 