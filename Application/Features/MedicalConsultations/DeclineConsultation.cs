using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using INotification = Application.Interfaces.INotification;

namespace Application.Features.MedicalConsultations
{
    public class DeclineConsultation
    {
        public class Command: IRequest<MedicalConsultationDto>
        {
            public int ConsultationId { get; set; }
            public string Comment { get; set; }
        }
        
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Comment).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, MedicalConsultationDto>
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
            public async Task<MedicalConsultationDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var consultation = await _context.MedicalConsultations
                    .Include(x => x.RequestedBy)
                    .Include(x => x.MedicalConsultationStatus)
                    .Include(x => x.RequestedDoctorUser.Doctor)
                    .FirstOrDefaultAsync(x => x.Id == request.ConsultationId, cancellationToken);

                if (consultation == null)
                {
                    throw new Exception("Consultation not found");
                }

                if (consultation.MedicalConsultationStatus.Description != "Pendente")
                {
                    throw new Exception("Consultation cannot be declined!");
                }

                var consultationStatus = await _context.MedicalConsultationStatus
                    .FirstOrDefaultAsync(x => x.Description == "Não Aceite", cancellationToken);

                if (consultationStatus == null)
                {
                    throw new Exception("Desired consultation Status not found");
                }

                consultation.MedicalConsultationStatus = consultationStatus;
                consultation.DoctorComment = request.Comment;
                consultation.DeclineDate = DateTimeOffset.Now;

                if (await _context.SaveChangesAsync(cancellationToken) > 0)
                {
                    var recipients = new List<string>();
                    recipients.Add(consultation.RequestedBy.Email);
                    await _notification.SendEmail(recipients,
                        $"A tua consulta com o {consultation.RequestedDoctorUser.Doctor.Speciality} - {consultation.RequestedDoctorUser.FirstName}," +
                        $"agendada para {consultation.BookDate.DateTime.ToLocalTime().ToString(CultureInfo.InvariantCulture)} infelizmente não foi aceite. Motivos: {consultation.DoctorComment}... Você pode entra na aplicação para mais detalhes",
                        $"Consulta Médica - Utomi Health Care");

                    return _mapper.Map<MedicalConsultation, MedicalConsultationDto>(consultation);
                }

                throw new Exception("Failed");
            }
        }
    }
}