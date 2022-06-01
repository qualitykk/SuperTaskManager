using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net;
using STaskManagerLibrary;

namespace STaskManagerApi.Controllers
{
    public partial class AccountController : Controller
    {
        private DbSet<STask> _tasks => _context.Task;

        [HttpGet("{user}/tasks")]
        public ActionResult<List<STask>> GetTasks(int user, string auth)
        {
            if (!IsTaskDataLoaded())
                return NotFound();

            var tasks = _tasks.Where(t => t.Account == user).ToList();
            return Ok(tasks);
        }
        private STask? TaskById(int user, int task)
        {
            return _tasks.Where(t => t.Account == user && t.Tid == task)?.FirstOrDefault(); ;
        }

        [HttpGet("{user}/tasks/{taskid}")]
        public ActionResult<STask> GetTask(int user, int taskid, string auth)
        {
            if (!IsTaskDataLoaded())
                return StatusCode(StatusCodes.Status500InternalServerError);

            var task = TaskById(user, taskid);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost("{user}/tasks")]
        public async Task<ActionResult> PostTask(int user, [FromBody] STask t)
        {
            if (!IsTaskDataLoaded())
                return StatusCode(StatusCodes.Status500InternalServerError);

            int newid = _tasks.Max(t => t.Tid) + 1;
            t.Tid = newid;

            await _tasks.AddAsync(t);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { user = user, taskid = newid }, t);
        }

        [HttpPatch("{user}/tasks/{taskid}")]
        public async Task<ActionResult> PatchTask(int user, int taskid, [FromBody] STask info)
        {
            if (!IsTaskDataLoaded())
                return StatusCode(StatusCodes.Status500InternalServerError);

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

        [HttpDelete("{user}/tasks/{taskid")]
        public ActionResult DeleteTask(int user, int taskid)
        {
            if (!IsTaskDataLoaded())
                return StatusCode(StatusCodes.Status500InternalServerError);

            var task = TaskById(user, taskid);
            if(task == null)
                return NotFound();

            _tasks.Remove(task);

            return Ok();
        }
    }
}
