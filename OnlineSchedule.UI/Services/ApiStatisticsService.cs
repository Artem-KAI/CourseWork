using System.Net.Http.Json;
using CONTRACT.DTO;

namespace UI.Services
{
    public class ApiStatisticsService
    {
        private readonly IHttpClientFactory factory;
        private readonly UserSessionService session;

        public ApiStatisticsService(
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

        public async Task<List<TeacherWorkloadDto>> GetTeacherStatisticsAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<TeacherWorkloadDto>>
                ("api/statistics/teachers") ?? new();
        }

        public async Task<List<ClassroomUtilizationDto>> GetClassroomStatisticsAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<ClassroomUtilizationDto>>
                ("api/statistics/classrooms") ?? new();
        }

        public async Task<List<GroupLoadDto>> GetGroupStatisticsAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<GroupLoadDto>>
                ("api/statistics/groups") ?? new();
        }
    }
}
