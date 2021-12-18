using System;
using Domain;

namespace Application.Dto
{
    public class MedicalConsultationDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DoctorDto RequestedDoctor { get; set; }
        public AppUserDto RequestedBy { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }
        public DateTimeOffset BookDate { get; set; }
        public decimal SpentHours { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DoctorConfirmationDate { get; set; }
        public MedicalConsultationStatus MedicalConsultationStatus { get; set; }
    }
}