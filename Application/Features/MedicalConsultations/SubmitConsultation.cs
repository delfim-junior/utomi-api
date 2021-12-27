using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Constants;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.MedicalConsultations
{
    public class SubmitConsultation
    {
        public class Command : IRequest<MedicalConsultationDto>
        {
            public int DoctorId { get; set; }
            public DateTimeOffset BookDate { get; set; }
            public string Description { get; set; }
        }
        
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Description).NotNull().NotEmpty();
                RuleFor(x => x.BookDate).NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Command, MedicalConsultationDto>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public Handler(DataContext context, UserManager<AppUser> userManager, IUserAccessor userAccessor, IMapper mapper)
            {
                _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }
            public async Task<MedicalConsultationDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var appUser = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserUserName(), cancellationToken);
                
                var requestedDoctorUser = await _context.Users
                    .Include(x=>x.Doctor)
                    .FirstOrDefaultAsync(x => x.Doctor.Id == request.DoctorId, cancellationToken);

                if (requestedDoctorUser == null)
                {
                    throw new Exception("Doctor not found");
                }

                var medicalStatus = await _context.MedicalConsultationStatus
                    .FirstOrDefaultAsync(x => x.Description == MedicalStatuses.Pending, cancellationToken);

                if (medicalStatus == null)
                {
                    throw new Exception("Pending status not found in Medical Consultation Statuses list");
                }

                var medicalConsultation = new MedicalConsultation
                {
                    Description = request.Description,
                    BookDate = request.BookDate,
                    RequestedBy = appUser,
                    RequestedDoctorUser = requestedDoctorUser,
                    SubmissionDate = DateTimeOffset.Now,
                    MedicalConsultationStatus = medicalStatus
                };

                await _context.MedicalConsultations.AddAsync(medicalConsultation, cancellationToken);

                if (await _context.SaveChangesAsync(cancellationToken) > 0)
                {
                    //Notify the Doctor
                    return _mapper.Map<MedicalConsultation, MedicalConsultationDto>(medicalConsultation);
                }

                throw new Exception("Failed to Submit Consultation");
            }
        }
    }
}