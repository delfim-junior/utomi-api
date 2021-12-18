using System;
using Domain;

namespace Application.Dto
{
    public class AppUserDto
    {
        public string PhotoUrl { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public DoctorDto Doctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdNumber { get; set; }
    }
}