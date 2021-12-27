using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.MedicalConsultations
{
    public class ListByUser
    {
        public class Query : IRequest<List<MedicalConsultationDto>>
        {
        }
        
        public class Handler : IRequestHandler<Query, List<MedicalConsultationDto>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager, IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _userManager = userManager;
                _mapper = mapper;
            }
            public async Task<List<MedicalConsultationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var loggedUser = await _userManager.Users
                    .Include(x => x.Doctor)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserUserName(), cancellationToken);

                if (loggedUser.Doctor != null)
                {
                    var consultations = await _context.MedicalConsultations
                        .Where(x=>x.RequestedDoctorUser.Doctor.Id == loggedUser.Doctor.Id)
                        .Include(x=>x.RequestedBy)
                        .Include(x=>x.RequestedDoctorUser.Doctor)
                        .Include(x=>x.MedicalConsultationStatus)
                        .ToListAsync(cancellationToken);

                    return _mapper.Map<List<MedicalConsultation>, List<MedicalConsultationDto>>(consultations);
                }
                else
                {
                    var consultations = await _context.MedicalConsultations
                        .Where(x => x.RequestedBy.Id == loggedUser.Id)
                        .Include(x => x.RequestedBy)
                        .Include(x => x.RequestedDoctorUser.Doctor)
                        .Include(x => x.MedicalConsultationStatus)
                        .ToListAsync(cancellationToken);
                    
                    return _mapper.Map<List<MedicalConsultation>, List<MedicalConsultationDto>>(consultations); 
                }
            }
        }
    }
}