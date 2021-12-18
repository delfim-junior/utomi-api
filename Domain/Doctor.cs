using System;

namespace Domain
{
    public class Doctor
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string Speciality { get; set; }
        public DateTime CareerStartDate { get; set; }
        public string AboutMe { get; set; }
        public string DeclineReason { get; set; }
        public DoctorRegistrationStatus DoctorRegistrationStatus { get; set; }
    }
}