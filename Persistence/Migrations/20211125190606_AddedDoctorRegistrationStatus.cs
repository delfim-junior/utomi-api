using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Persistence.Migrations
{
    public partial class AddedDoctorRegistrationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorRegistrationStatusId",
                table: "Doctors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DoctorRegistrationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorRegistrationStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DoctorRegistrationStatusId",
                table: "Doctors",
                column: "DoctorRegistrationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorRegistrationStatuses_DoctorRegistrationStatus~",
                table: "Doctors",
                column: "DoctorRegistrationStatusId",
                principalTable: "DoctorRegistrationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorRegistrationStatuses_DoctorRegistrationStatus~",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "DoctorRegistrationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DoctorRegistrationStatusId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DoctorRegistrationStatusId",
                table: "Doctors");
        }
    }
}
