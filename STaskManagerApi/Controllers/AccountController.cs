using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STaskManagerLibrary;

namespace STaskManagerApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public partial class AccountController : Controller
    {
        private static TaskManagerContext _context = new();
        private static bool IsTaskDataLoaded()
        {
            try
            {
                return _context != null && _context.Database.CanConnect();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        protected StatusCodeResult InternalError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
