using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Persistence;
using INotification = Application.Interfaces.INotification;

namespace Application.Features.Doctors
{
    public class CreateDoctor
    {
        public class Command : IRequest<DoctorDto>
        {
            public string Gender { get; set; }
            public DateTime BirthDate { get; set; }
            public string Address { get; set; }
            public string OrderNumber { get; set; }
            public string Speciality { get; set; }
            public DateTime CareerStartDate { get; set; }
            public string AboutMe { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string IdNumber { get; set; }
            public IFormFile File { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Gender);
                RuleFor(x => x.BirthDate);
                RuleFor(x => x.OrderNumber);
                RuleFor(x => x.Speciality);
                RuleFor(x => x.CareerStartDate).LessThanOrEqualTo(x => DateTime.Now);
                RuleFor(x => x.FirstName);
                RuleFor(x => x.LastName);
                RuleFor(x => x.Email);
                RuleFor(x => x.PhoneNumber);
            }
        }

        public class Handler : IRequestHandler<Command, DoctorDto>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;
            private readonly INotification _notification;
            private readonly IFileHandler _fileHandler;

            public Handler(DataContext context, UserManager<AppUser> userManager, IMapper mapper,
                INotification notification, IFileHandler fileHandler)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
                _notification = notification;
                _fileHandler = fileHandler;
            }

            public async Task<DoctorDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var doctor = new Doctor
                {
                    Speciality = request.Speciality,
                    AboutMe = request.AboutMe,
                    OrderNumber = request.OrderNumber,
                    CareerStartDate = request.CareerStartDate,
                };

                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                await _context.Doctors.AddAsync(doctor, cancellationToken);

                if (!(await _context.SaveChangesAsync(cancellationToken) > 0))
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception("Failed to create Doctor");
                }

                var appUser = new AppUser
                {
                    Address = request.Address,
                    Doctor = doctor,
                    Email = request.Email,
                    Gender = request.Gender.ToLower().Contains("f") ? Sex.Female.ToString() : Sex.Male.ToString(),
                    BirthDate = request.BirthDate,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    UserName = $"{request.FirstName.ToLower()}.{request.LastName.ToLower()}",
                    IdNumber = request.IdNumber
                };

                var result = await _userManager.CreateAsync(appUser, "1Secure*Password1");

                var recipients = new List<string>();
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception("Failed to create Doctor");
                }

                if (request.File != null)
                {
                    try
                    {
                        var filePath = await _fileHandler.WriteFile(request.File, "UploadDir");
                        
                        var doctorDocument = new DoctorDocument
                        {
                            Doctor = doctor,
                            Name = request.File.FileName,
                            Url = filePath
                        };

                        await _context.DoctorDocuments.AddAsync(doctorDocument, cancellationToken);

                        if (await _context.SaveChangesAsync(cancellationToken) <= 0)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            throw new Exception("Failed to Save Document");
                        }
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        throw new Exception(e.Message);
                    }
                }
                
                await transaction.CommitAsync(cancellationToken);
                    
                recipients.Add(request.Email);

                var notificationResponse = await _notification.SendEmail(recipients, "You are successully registed to UTOMI App",
                    $"Doctor Registration - {request.FirstName}");

                return new DoctorDto
                {
                    Id = doctor.Id,
                    Speciality = doctor.Speciality,
                    AboutMe = doctor.AboutMe,
                    ExperienceYears = DateTime.Now.Year - doctor.CareerStartDate.Year,
                    OrderNumber = doctor.OrderNumber,
                    User = _mapper.Map<AppUser, AppUserDto>(appUser)
                };
            }
        }
    }
}