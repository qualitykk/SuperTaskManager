using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace STaskManagerApi.Controllers
{
    [Route("api/accounts/")]
    [ApiController]
    public partial class AccountController : ControllerBase
    {
        private static TaskManagerContext _context = new();
        private static bool IsTaskDataLoaded()
        {
            return _context != null;
        }

        private static bool IsAccountDataLoaded()
        {
            return Service.Auth.IsActive();
        }
    }
}
