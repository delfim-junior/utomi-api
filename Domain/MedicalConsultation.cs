using System;

namespace Domain
{
    public class MedicalConsultation
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public AppUser RequestedDoctorUser { get; set; }
        public AppUser RequestedBy { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }
        public DateTimeOffset BookDate { get; set; }
        public decimal SpentHours { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DoctorConfirmationDate { get; set; }
        public MedicalConsultationStatus MedicalConsultationStatus { get; set; }
        public string DoctorComment { get; set; }
        public AppUser AcceptedBy { get; set; }
        public DateTimeOffset? AcceptanceDate { get; set; }
        public AppUser DeclinedBy { get; set; }
        public DateTimeOffset? DeclineDate { get; set; }
    }
}