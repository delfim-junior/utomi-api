using System.Net;
using Application.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]
    public class ErrorsController : BaseController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse((HttpStatusCode)code));
        }
    }
}