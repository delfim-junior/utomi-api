using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Persistence.Migrations
{
    public partial class AddedMedicalConsultationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalConsultationStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalConsultationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalConsultations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    RequestedDoctorId = table.Column<int>(type: "integer", nullable: true),
                    RequestedById = table.Column<string>(type: "text", nullable: true),
                    SubmissionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    BookDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SpentHours = table.Column<decimal>(type: "numeric", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DoctorConfirmationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MedicalConsultationStatusId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalConsultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalConsultations_AspNetUsers_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalConsultations_Doctors_RequestedDoctorId",
                        column: x => x.RequestedDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalConsultations_MedicalConsultationStatus_MedicalConsu~",
                        column: x => x.MedicalConsultationStatusId,
                        principalTable: "MedicalConsultationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultations_MedicalConsultationStatusId",
                table: "MedicalConsultations",
                column: "MedicalConsultationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultations_RequestedById",
                table: "MedicalConsultations",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultations_RequestedDoctorId",
                table: "MedicalConsultations",
                column: "RequestedDoctorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalConsultations");

            migrationBuilder.DropTable(
                name: "MedicalConsultationStatus");
        }
    }
}
