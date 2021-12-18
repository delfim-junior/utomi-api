using System.Threading.Tasks;
using Application.Dto;
using Application.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AppUserDto>> Create(CreateUser.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}