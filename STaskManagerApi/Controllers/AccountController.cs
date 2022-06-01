using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace STaskManagerApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public partial class AccountController : Controller
    {
        private static TaskManagerContext _context = new();
        private static bool IsTaskDataLoaded()
        {
            return _context != null;
        }
    }
}
