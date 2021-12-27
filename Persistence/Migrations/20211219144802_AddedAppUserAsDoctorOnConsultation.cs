using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddedAppUserAsDoctorOnConsultation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConsultations_Doctors_RequestedDoctorId",
                table: "MedicalConsultations");

            migrationBuilder.DropIndex(
                name: "IX_MedicalConsultations_RequestedDoctorId",
                table: "MedicalConsultations");

            migrationBuilder.DropColumn(
                name: "RequestedDoctorId",
                table: "MedicalConsultations");

            migrationBuilder.AddColumn<string>(
                name: "RequestedDoctorUserId",
                table: "MedicalConsultations",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultations_RequestedDoctorUserId",
                table: "MedicalConsultations",
                column: "RequestedDoctorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConsultations_AspNetUsers_RequestedDoctorUserId",
                table: "MedicalConsultations",
                column: "RequestedDoctorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConsultations_AspNetUsers_RequestedDoctorUserId",
                table: "MedicalConsultations");

            migrationBuilder.DropIndex(
                name: "IX_MedicalConsultations_RequestedDoctorUserId",
                table: "MedicalConsultations");

            migrationBuilder.DropColumn(
                name: "RequestedDoctorUserId",
                table: "MedicalConsultations");

            migrationBuilder.AddColumn<int>(
                name: "RequestedDoctorId",
                table: "MedicalConsultations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultations_RequestedDoctorId",
                table: "MedicalConsultations",
                column: "RequestedDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConsultations_Doctors_RequestedDoctorId",
                table: "MedicalConsultations",
                column: "RequestedDoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
