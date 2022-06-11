using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STaskManagerLibrary;

namespace STaskManagerApi.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<LoginResponse>> GetLogin([FromBody] LoginRequest auth)
        {
            if(!Service.Auth.IsActive())
                return StatusCode(StatusCodes.Status500InternalServerError);

            var response = await Service.Auth.TryLogon(auth.Username, auth.Password);
            if(!response)
                return StatusCode(StatusCodes.Status403Forbidden);
            else
                return Ok(response);
        }
    }
}
