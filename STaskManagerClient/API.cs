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
        private static bool loggedIn => userId != default;

        private const string ApiUrl = "https://localhost:5001/api/";
        private static HttpClient client = new HttpClient() { BaseAddress = new Uri(ApiUrl) };
        private static JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        private static StringContent Jsonify(object obj)
        {
            return new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        private static async Task<T> ObjectifyAsync<T>(HttpResponseMessage msg)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return JsonSerializer.Deserialize<T>(await msg.Content.ReadAsStringAsync(), options);
#pragma warning restore CS8603 // Possible null reference return.
        }
        /// <summary>
        /// Logs in with username and hashed password
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="password">Hashed password</param>
        /// <returns>Success</returns>
        /// <exception cref="HttpRequestException"
        public static async Task<bool> LoginAsync(string user, string password)
        {
            LoginRequest login = new(user, password);
            var content = Jsonify(login);

            var request = await client.PostAsync("/login", content);
            if (!request.IsSuccessStatusCode)
                return false;

            var response = await ObjectifyAsync<LoginResponse>(request);
            if (!response)
                return false;

#nullable disable
            userId = (int)response.Userid;
            auth = (long)response.Auth;
            client.BaseAddress = new Uri(ApiUrl + $"{userId}/");
#nullable restore

            return true;
        }

        public static async Task<List<STask>?> GetTasksAsync()
        {
            if (!loggedIn)
                return null;

            var request = await client.GetAsync("tasks");
            if (!request.IsSuccessStatusCode)
                return null;

            var response = await ObjectifyAsync<List<STask>>(request);
            return response;
        }
        public static async Task<STask?> GetTaskAsync(int id)
        {
            if (!loggedIn)
                return null;

            var request = await client.GetAsync($"tasks/{id}");
            if (!request.IsSuccessStatusCode)
                return null;

            var response = await ObjectifyAsync<STask>(request);
            return response;
        }
        /// <summary>
        /// Requests task creation
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Success</returns>
        public static async Task<bool> AddTaskAsync(STask task)
        {
            if (!loggedIn)
                return false;

            var content = Jsonify(task);
            var request = await client.PostAsync("tasks", content);
            if (!request.IsSuccessStatusCode)
                return false;

            return true;
        }
        public static async Task<bool> UpdateTaskAsync(int id, STask newInfo)
        {
            if (!loggedIn)
                return false;

            var content = Jsonify(newInfo);
            var request = await client.PatchAsync($"tasks/{id}", content);
            if (!request.IsSuccessStatusCode)
                return false;

            return true;
        }

        public static async Task<bool> DeleteTaskAsync(int id)
        {
            if (!loggedIn)
                return false;

            var request = await client.DeleteAsync($"tasks/{id}");
            if (!request.IsSuccessStatusCode)
                return false;

            return true;
        }
        public static async Task<List<Category>?> GetCategoriesAsync()
        {
            var request = await client.GetAsync("categories");
            if (!request.IsSuccessStatusCode)
                return null;

            var response = await ObjectifyAsync<List<Category>>(request);
            return response;
        }
    }
}
