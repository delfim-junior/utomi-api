using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddedLogFieldsToConsultation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AcceptanceDate",
                table: "MedicalConsultations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptedById",
                table: "MedicalConsultations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeclineDate",
                table: "MedicalConsultations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeclinedById",
                table: "MedicalConsultations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorComment",
                table: "MedicalConsultations",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultations_AcceptedById",
                table: "MedicalConsultations",
                column: "AcceptedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultations_DeclinedById",
                table: "MedicalConsultations",
                column: "DeclinedById");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConsultations_AspNetUsers_AcceptedById",
                table: "MedicalConsultations",
                column: "AcceptedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConsultations_AspNetUsers_DeclinedById",
                table: "MedicalConsultations",
                column: "DeclinedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConsultations_AspNetUsers_AcceptedById",
                table: "MedicalConsultations");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConsultations_AspNetUsers_DeclinedById",
                table: "MedicalConsultations");

            migrationBuilder.DropIndex(
                name: "IX_MedicalConsultations_AcceptedById",
                table: "MedicalConsultations");

            migrationBuilder.DropIndex(
                name: "IX_MedicalConsultations_DeclinedById",
                table: "MedicalConsultations");

            migrationBuilder.DropColumn(
                name: "AcceptanceDate",
                table: "MedicalConsultations");

            migrationBuilder.DropColumn(
                name: "AcceptedById",
                table: "MedicalConsultations");

            migrationBuilder.DropColumn(
                name: "DeclineDate",
                table: "MedicalConsultations");

            migrationBuilder.DropColumn(
                name: "DeclinedById",
                table: "MedicalConsultations");

            migrationBuilder.DropColumn(
                name: "DoctorComment",
                table: "MedicalConsultations");
        }
    }
}
