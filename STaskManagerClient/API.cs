using STaskManagerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerClient
{
    internal static class API
    {
        private static int userId;
        private static string? auth;
        /// <summary>
        /// Logs in with username and hashed password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>Success</returns>
        public static async Task<bool> LoginAsync(string user, string password)
        {
            throw new NotImplementedException();
        }

        public static async Task<List<STask>> GetTasksAsync()
        {
            throw new NotImplementedException();
        }
        public static async Task<STask> GetTaskAsync(int id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Requests task creation
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Success</returns>
        public static async Task<bool> AddTaskAsync(STask task)
        {
            throw new NotImplementedException();
        }
        public static async Task<bool> UpdateTaskAsync(int id, STask newInfo)
        {
            throw new NotImplementedException();
        }

        public static async Task<bool> DeleteTaskAsync(int id)
        {
            throw new NotImplementedException();
        }
        public static async Task<Category> GetCategoriesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
