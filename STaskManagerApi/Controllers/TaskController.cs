using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STaskManagerLibrary;

namespace STaskManagerApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public partial class TaskController : ControllerBase
    {
        private static readonly TaskManagerContext _context = new();
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

        #region Helpers
        protected StatusCodeResult InternalError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        protected static Account? GetUser(int id)
        {
            return _context.Account.FirstOrDefault(a => a.Uid == id);
        }
        #endregion

        [HttpGet("categories")]
        public ActionResult<List<Category>> GetCategories()
        {
            if (!IsTaskDataLoaded())
                return InternalError();

            return Ok(_context.Category);
        }


        private static DbSet<STask> tasks => _context.Task;

        [HttpGet("{user}")]
        public ActionResult<List<STask>> GetTasks(int user)
        {
            if (!IsTaskDataLoaded())
                return InternalError();

            var match = tasks.Where(t => t.Account == user).ToList();
            return Ok(tasks);
        }
        protected static STask? TaskById(int user, int task)
        {
            return tasks.Where(t => t.Account == user && t.Tid == task)?.FirstOrDefault(); ;
        }

        [HttpGet("{user}/{taskid}")]
        public ActionResult<STask> GetTask(int user, int taskid)
        {
            if (!IsTaskDataLoaded())
                return InternalError();

            var task = TaskById(user, taskid);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost("{user}")]
        public async Task<ActionResult> PostTask(int user, [FromBody] STask t)
        {
            if (!IsTaskDataLoaded())
                return InternalError();

            int newid = tasks.Max(t => t.Tid) + 1;
            t.Tid = newid;

            await tasks.AddAsync(t);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { user, taskid = newid }, t);
        }

        [HttpPatch("{user}/{taskid}")]
        public async Task<ActionResult> PatchTask(int user, int taskid, [FromBody] STask info)
        {
            if (!IsTaskDataLoaded())
                return InternalError();

            var task = TaskById(user, taskid);
            if (task == null)
                return NotFound();

            task.Name = info.Name;
            task.Description = info.Description;
            task.Priority = info.Priority;

            task.Belongsto = info.Belongsto;
            task.Required = info.Required;
            task.Duedate = info.Duedate;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{user}/{taskid}")]
        public ActionResult DeleteTask(int user, int taskid)
        {
            if (!IsTaskDataLoaded())
                return InternalError();

            var task = TaskById(user, taskid);
            if (task == null)
                return NotFound();

            tasks.Remove(task);

            return Ok();
        }
    }
}
