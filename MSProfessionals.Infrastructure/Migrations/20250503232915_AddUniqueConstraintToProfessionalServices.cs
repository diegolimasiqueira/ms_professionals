using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSProfessionals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToProfessionalServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add unique constraint for professional services
            migrationBuilder.CreateIndex(
                name: "UQ_tb_professional_services_professional_profession_id_service_id",
                schema: "shc_professional",
                table: "tb_professional_services",
                columns: new[] { "professional_profession_id", "service_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove unique constraint for professional services
            migrationBuilder.DropIndex(
                name: "UQ_tb_professional_services_professional_profession_id_service_id",
                schema: "shc_professional",
                table: "tb_professional_services");
        }
    }
} 