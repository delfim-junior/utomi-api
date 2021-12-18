using System;
using Application.Dto;
using AutoMapper;
using Domain;

namespace API.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.ExperienceYears,
                    opt => opt.MapFrom(x => DateTime.Now.Year - x.CareerStartDate.Year));
            CreateMap<AppUser, AppUserDto>();
            CreateMap<MedicalConsultation, MedicalConsultationDto>();
        }
    }
}