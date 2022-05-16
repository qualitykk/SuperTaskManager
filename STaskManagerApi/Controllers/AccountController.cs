using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace STaskManagerApi.Controllers
{
    [Route("api/accounts/")]
    [ApiController]
    public partial class AccountController : ControllerBase
    {
        private static TaskManagerContext _context = new();
    }
}
