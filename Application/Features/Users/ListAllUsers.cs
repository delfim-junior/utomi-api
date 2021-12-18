using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Users
{
    public class ListAllUsers
    {
        public class Query : IRequest<List<AppUserDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<AppUserDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<AppUserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users
                    .Where(x => x.Doctor != null)
                    .Include(x => x.Doctor)
                    .Select(x => new AppUserDto
                    {
                        Address = x.Address,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        BirthDate = x.BirthDate,
                        Gender = x.Gender,
                        LastName = x.LastName,
                        PhoneNumber = x.PhoneNumber,
                        PhotoUrl = x.PhotoUrl,
                        Doctor = new DoctorDto
                        {
                            Id = x.Doctor.Id,
                            Speciality = x.Doctor.Speciality,
                            AboutMe = x.Doctor.AboutMe,
                            ExperienceYears = DateTime.Now.Year - x.Doctor.CareerStartDate.Year,
                            OrderNumber = x.Doctor.OrderNumber
                        }
                    })
                    .ToListAsync(cancellationToken);

                return users;
            }
        }
    }
}