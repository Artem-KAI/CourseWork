using System.Net.Http.Json;
using UI.Models;
using CONTRACT.DTO;

namespace UI.Services
{
    public class ApiUserService
    {
        private readonly IHttpClientFactory factory;
        private readonly UserSessionService session;

        public ApiUserService(
            IHttpClientFactory factory,
            UserSessionService session)
        {
            this.factory = factory;
            this.session = session;
        }

        private HttpClient CreateClient()
        {
            var client = factory.CreateClient("ScheduleAPI");

            if (session.CurrentUser != null)
            {
                client.DefaultRequestHeaders.Add(
                    "X-User-Id",
                    session.CurrentUser.Id.ToString());
            }

            return client;
        }

        public async Task<UserInfo?> CreateUserAsync(RegisterRequest request)
        {
            var client = CreateClient();

            var response =
                await client.PostAsJsonAsync(
                    "api/users",
                    request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<UserInfo>();
        }

        public async Task<List<UserInfo>> GetUsersAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<UserInfo>>
                ("api/users") ?? new();
        }

        public async Task<UserInfo?> GetUserAsync(int id)
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<UserInfo>
                ($"api/users/{id}");
        }

        public async Task UpdateUserAsync(int id, UpdateUserRequests request)
        {
            var client = CreateClient();

            await client.PutAsJsonAsync(
                $"api/users/{id}",
                request);
        }

        public async Task DeleteUserAsync(int id)
        {
            var client = CreateClient();

            await client.DeleteAsync($"api/users/{id}");
        }
    }
}
