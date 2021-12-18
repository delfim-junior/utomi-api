using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Features.Doctors;
using BrunoZell.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DoctorsController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<DoctorDto>> Create([ModelBinder(BinderType = typeof(JsonModelBinder))]
            CreateDoctor.Command command,
            [FromForm] IFormFile file)
        {
            command.File = file;
            return await Mediator.Send(command);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<DoctorDto>>> ListAll()
        {
            return await Mediator.Send(new ListAllDoctors.Query());
        }

        [HttpPut("{doctorId:int}/Approval")]
        public async Task<ActionResult<DoctorDto>> Approve(int doctorId)
        {
            return await Mediator.Send(new ApproveDoctor.Command {DoctorId = doctorId});
        }

        [HttpPut("{doctorId:int}/Rejection")]
        public async Task<ActionResult<DoctorDto>> Reject(int doctorId)
        {
            return await Mediator.Send(new DeclineDoctor.Command {DoctorId = doctorId});
        }
    }
}