using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.MedicalConsultations
{
    public class ListAll
    {
        public class Query : IRequest<List<MedicalConsultationDto>>
        {
        }
        
        public class Handler : IRequestHandler<Query,List<MedicalConsultationDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<MedicalConsultationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var consultations = await _context.MedicalConsultations
                    .Include(x=>x.RequestedBy)
                    .Include(x=>x.RequestedDoctor)
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<MedicalConsultation>, List<MedicalConsultationDto>>(consultations);
            }
        }
    }
}