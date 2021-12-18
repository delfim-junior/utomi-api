using System;

namespace Domain
{
    public class MedicalConsultation
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Doctor RequestedDoctor { get; set; }
        public AppUser RequestedBy { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }
        public DateTimeOffset BookDate { get; set; }
        public decimal SpentHours { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DoctorConfirmationDate { get; set; }
        public MedicalConsultationStatus MedicalConsultationStatus { get; set; }
    }
}