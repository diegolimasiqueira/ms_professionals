using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSProfessionals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shc_professional");

            migrationBuilder.CreateTable(
                name: "tb_country_codes",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    country_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_country_codes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_currencies",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_currencies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_languages",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_professions",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_professions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_services",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_time_zones",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_time_zones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_professionals",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    document_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    photo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    social_media = table.Column<string>(type: "jsonb", nullable: true),
                    media = table.Column<string>(type: "jsonb", nullable: true),
                    currency_id = table.Column<Guid>(type: "uuid", nullable: false),
                    phone_country_code_id = table.Column<Guid>(type: "uuid", nullable: false),
                    preferred_language_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timezone_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_professionals", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_professionals_tb_country_codes_phone_country_code_id",
                        column: x => x.phone_country_code_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_country_codes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_professionals_tb_currencies_currency_id",
                        column: x => x.currency_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_currencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_professionals_tb_languages_preferred_language_id",
                        column: x => x.preferred_language_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_professionals_tb_time_zones_timezone_id",
                        column: x => x.timezone_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_time_zones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_professional_address",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_id = table.Column<Guid>(type: "uuid", nullable: false),
                    street_address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    city = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    postalcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    country_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_professional_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_professional_address_tb_country_codes_country_id",
                        column: x => x.country_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_country_codes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_professional_address_tb_professionals_professional_id",
                        column: x => x.professional_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_professionals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_professional_professions",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profession_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_professional_professions", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_professional_professions_tb_professionals_professional_id",
                        column: x => x.professional_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_professionals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_professional_professions_tb_professions_profession_id",
                        column: x => x.profession_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_professions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_professional_services",
                schema: "shc_professional",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profession_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_professional_services", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_professional_services_tb_professional_professions_professional_profession_id",
                        column: x => x.professional_profession_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_professional_professions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_professional_services_tb_services_service_id",
                        column: x => x.service_id,
                        principalSchema: "shc_professional",
                        principalTable: "tb_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_professional_address_country_id",
                schema: "shc_professional",
                table: "tb_professional_address",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professional_address_professional_id",
                schema: "shc_professional",
                table: "tb_professional_address",
                column: "professional_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professional_professions_profession_id",
                schema: "shc_professional",
                table: "tb_professional_professions",
                column: "profession_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professional_professions_professional_id",
                schema: "shc_professional",
                table: "tb_professional_professions",
                column: "professional_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professional_services_professional_profession_id",
                schema: "shc_professional",
                table: "tb_professional_services",
                column: "professional_profession_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professional_services_service_id",
                schema: "shc_professional",
                table: "tb_professional_services",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professionals_currency_id",
                schema: "shc_professional",
                table: "tb_professionals",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professionals_document_id",
                schema: "shc_professional",
                table: "tb_professionals",
                column: "document_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_professionals_email",
                schema: "shc_professional",
                table: "tb_professionals",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_professionals_phone_country_code_id",
                schema: "shc_professional",
                table: "tb_professionals",
                column: "phone_country_code_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professionals_phone_number",
                schema: "shc_professional",
                table: "tb_professionals",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_professionals_preferred_language_id",
                schema: "shc_professional",
                table: "tb_professionals",
                column: "preferred_language_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_professionals_timezone_id",
                schema: "shc_professional",
                table: "tb_professionals",
                column: "timezone_id");

            migrationBuilder.CreateIndex(
                name: "UQ_tb_professions_name",
                schema: "shc_professional",
                table: "tb_professions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_tb_services_name",
                schema: "shc_professional",
                table: "tb_services",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_professional_address",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_professional_services",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_professional_professions",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_services",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_professionals",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_professions",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_country_codes",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_currencies",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_languages",
                schema: "shc_professional");

            migrationBuilder.DropTable(
                name: "tb_time_zones",
                schema: "shc_professional");
        }
    }
}
