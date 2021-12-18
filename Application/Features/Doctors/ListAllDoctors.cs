using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Doctors
{
    public class ListAllDoctors
    {
        public class Query : IRequest<List<DoctorDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<DoctorDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<DoctorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var doctors = await _context.Users
                    .Where(x=>x.Doctor != null)
                    .Include(x=>x.Doctor.DoctorRegistrationStatus)
                    .Select(x => new DoctorDto
                    {
                        Id = x.Doctor.Id,
                        Speciality = x.Doctor.Speciality,
                        AboutMe = x.Doctor.AboutMe,
                        OrderNumber = x.Doctor.OrderNumber,
                        DoctorRegistrationStatus = x.Doctor.DoctorRegistrationStatus,
                        User = _mapper.Map<AppUser, AppUserDto>(x)
                    })
                    .ToListAsync(cancellationToken);

                return doctors;
            }
        }
    }
}