using STaskManagerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace STaskManagerClient
{
    internal static class API
    {
        private static int userId;
        private static long auth;

        private const string ApiUrl = "https://localhost:7242/api/";
        private static HttpClient client = new HttpClient() { BaseAddress = new Uri(ApiUrl) };
        private static JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Logs in with username and hashed password
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="password">Hashed password</param>
        /// <returns>Success</returns>
        public static async Task<bool> LoginAsync(string user, string password)
        {
            LoginRequest request = new(user, password);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            try
            {
                var answer = await client.PostAsync("/login", content);
                if (!answer.IsSuccessStatusCode)
                    return false;

                var response = JsonSerializer.Deserialize<LoginResponse>(await answer.Content.ReadAsStringAsync(), options);
                if (!response)
                    return false;

#nullable disable
                userId = (int)response.Userid;
                auth = (long)response.Auth;
#nullable restore

                return true;
            }
            catch(HttpRequestException)
            {

            }
            return false;
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
