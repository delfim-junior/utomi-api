using System.Threading.Tasks;
using Application.Dto;
using Application.Features.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(Login.Command command)
        {
            // return new LoginResponse
            // {
            //     Email = "delfas@gmail.com"
            // }; 
            return await Mediator.Send(command);
        }
    }
}