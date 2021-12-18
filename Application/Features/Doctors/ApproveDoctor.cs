using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using INotification = Application.Interfaces.INotification;

namespace Application.Features.Doctors
{
    public class ApproveDoctor
    {
        public class Command : IRequest<DoctorDto>
        {
            public int DoctorId { get; set; }
        }

        public class Handler : IRequestHandler<Command, DoctorDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly INotification _notification;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, INotification notification,
                UserManager<AppUser> userManager, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _notification = notification;
                _userManager = userManager;
                _userAccessor = userAccessor;
            }

            public async Task<DoctorDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users
                    .Include(x => x.Doctor)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserUserName(), cancellationToken);
                
                var doctor = await _context.Doctors
                    .Include(x => x.DoctorRegistrationStatus)
                    .FirstOrDefaultAsync(x => x.Id == request.DoctorId, cancellationToken);

                if (doctor == null)
                {
                    throw new Exception("Doctor not found");
                }

                var registrationStatus = await _context.DoctorRegistrationStatuses
                    .FirstOrDefaultAsync(x => x.Description.Equals(DoctorRegistrationStatusesConstants.Approved),
                        cancellationToken);

                if (registrationStatus == null)
                {
                    throw new Exception("Required Status not found!");
                }

                doctor.DoctorRegistrationStatus = registrationStatus;

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
                        "You are successfully Approved as to UTOMI App Doctor. Now you can enter and receive consultation",
                        $"Doctor Registration - {doctorDto.User.FirstName}");

                    return doctorDto;
                }

                throw new Exception("Failed to Approve Doctor");
            }
        }
    }
}