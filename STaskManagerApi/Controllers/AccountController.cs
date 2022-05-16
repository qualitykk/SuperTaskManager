using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace STaskManagerApi.Controllers
{
    [Route("api/áccounts/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static TaskManagerContext _context = new();
        [HttpGet("{user}/tasks")]
        public ActionResult<List<Task>> GetTasks(int user)
        {
            if(_context == null)
                return NotFound();

            return Ok(_context.Task.Where(t => t.Account == user).ToList());
        }
    }
}
