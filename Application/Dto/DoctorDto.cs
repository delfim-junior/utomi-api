using System;
using Domain;

namespace Application.Dto
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string Speciality { get; set; }
        public int ExperienceYears { get; set; }
        public string AboutMe { get; set; }
        public string DeclineReason { get; set; }
        public DoctorRegistrationStatus DoctorRegistrationStatus { get; set; }

        public AppUserDto User { get; set; }
    }
}