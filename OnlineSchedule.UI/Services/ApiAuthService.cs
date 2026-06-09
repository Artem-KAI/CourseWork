using System.Net.Http.Json;
using UI.Models;

namespace UI.Services
{
    public class ApiAuthService
    {
        private readonly IHttpClientFactory factory;

        public ApiAuthService(IHttpClientFactory factory)
        {
            this.factory = factory;
        }

        public async Task<CurrentUser?> LoginAsync(LoginRequest request)
        {
            var client = factory.CreateClient("ScheduleAPI");

            var response = await client.PostAsJsonAsync(
                "api/auth/login",
                request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response
                .Content
                .ReadFromJsonAsync<CurrentUser>();
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var client = factory.CreateClient("ScheduleAPI");

            var response = await client.PostAsJsonAsync(
                "api/auth/register",
                request);

            return response.IsSuccessStatusCode;
        }
    }
}
