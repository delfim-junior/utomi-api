using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Features.MedicalConsultations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MedicalConsultationsController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<MedicalConsultationDto>> Submit(SubmitConsultation.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<MedicalConsultationDto>>> ListAll()
        {
            return await Mediator.Send(new ListAll.Query());
        }
        
        [HttpGet("ByUser")]
        public async Task<ActionResult<List<MedicalConsultationDto>>> GetByUser()
        {
            return await Mediator.Send(new ListByUser.Query());
        }

        [HttpPut("Acceptance")]
        public async Task<ActionResult<MedicalConsultationDto>> Accept(AcceptConsultation.Command command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpPut("Decline")]
        public async Task<ActionResult<MedicalConsultationDto>> Decline(DeclineConsultation.Command command)
        {
            return await Mediator.Send(command);
        }
        
    }
}