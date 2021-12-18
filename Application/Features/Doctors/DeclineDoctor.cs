using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using AutoMapper;
using Domain;
using Domain.Constants;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using INotification = Application.Interfaces.INotification;

namespace Application.Features.Doctors
{
    public class DeclineDoctor
    {
        public class Command : IRequest<DoctorDto>
        {
            public int DoctorId { get; set; }
            public string DeclineReason { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.DeclineReason).NotEmpty().Null();
            }
        }
        public class Handler : IRequestHandler<Command, DoctorDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly INotification _notification;

            public Handler(DataContext context, IMapper mapper, INotification notification)
            {
                _context = context;
                _mapper = mapper;
                _notification = notification;
            }

            public async Task<DoctorDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var doctor = await _context.Doctors
                    .Include(x => x.DoctorRegistrationStatus)
                    .FirstOrDefaultAsync(x => x.Id == request.DoctorId, cancellationToken);

                if (doctor == null)
                {
                    throw new Exception("Doctor not found");
                }

                var registrationStatus = await _context.DoctorRegistrationStatuses
                    .FirstOrDefaultAsync(x => x.Description.Equals(DoctorRegistrationStatusesConstants.Declined),
                        cancellationToken);

                if (registrationStatus == null)
                {
                    throw new Exception("Required Status not found!");
                }

                doctor.DoctorRegistrationStatus = registrationStatus;
                doctor.DeclineReason = request.DeclineReason;

                if (await _context.SaveChangesAsync(cancellationToken) > 0)
                {
                    var doctorDto = await _context.Users
                        .Where(x => x.Doctor.Id == request.DoctorId)
                        .Include(x => x.Doctor.DoctorRegistrationStatus)
                        .Select(x => new DoctorDto
                        {
                            Id = x.Doctor.Id,
                            Speciality = x.Doctor.Speciality,
                            AboutMe = x.Doctor.AboutMe,
                            OrderNumber = x.Doctor.OrderNumber,
                            DoctorRegistrationStatus = x.Doctor.DoctorRegistrationStatus,
                            User = _mapper.Map<AppUser, AppUserDto>(x)
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    var recipients = new List<string>();
                    recipients.Add(doctorDto.User.Email);
                    await _notification.SendEmail(recipients,
                        $"Unfortunately your request to be UTOMI Platform Doctor were {registrationStatus.Description} " +
                        $"for the following reason: {request.DeclineReason}.",
                        $"Doctor Registration - {doctorDto.User.FirstName}");

                    return doctorDto;
                }

                throw new Exception("Failed to Decline Doctor");
            }
        }
    }
}