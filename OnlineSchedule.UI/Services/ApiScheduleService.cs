using CONTRACT.DTO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UI.Services
{
    public class ApiScheduleService
    {
        private readonly IHttpClientFactory factory;
        private readonly UserSessionService session;

        public ApiScheduleService(
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

        private readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        private async Task<T?> GetAsync<T>(string url)
        {
            var client = CreateClient();

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return default;

            return await response.Content
                .ReadFromJsonAsync<T>(jsonOptions);
        }

        public async Task<List<GroupInfo>> GetGroupsAsync()
        {
            return await GetAsync<List<GroupInfo>>
                ("api/schedule/groups") ?? new();
        }

        public async Task<List<TeacherInfo>> GetTeachersAsync()
        {
            return await GetAsync<List<TeacherInfo>>
                ("api/schedule/teachers") ?? new();
        }

        public async Task<List<ClassroomInfo>> GetClassroomsAsync()
        {
            return await GetAsync<List<ClassroomInfo>>
                ("api/schedule/classrooms") ?? new();
        }

        public async Task<List<DepartmentInfo>> GetDepartmentsAsync()
        {
            return await GetAsync<List<DepartmentInfo>>
                ("api/schedule/departments") ?? new();
        }

        public async Task<List<DisciplineInfo>> GetDisciplinesAsync()
        {
            return await GetAsync<List<DisciplineInfo>>
                ("api/schedule/disciplines") ?? new();
        }

        public async Task<List<ScheduleItemInfo>>
            GetScheduleForGroupAsync(int groupId)
        {
            return await GetAsync<List<ScheduleItemInfo>>
                ($"api/schedule/group/{groupId}") ?? new();
        }

        public async Task<List<ScheduleItemInfo>>
            GetScheduleForTeacherAsync(int teacherId)
        {
            return await GetAsync<List<ScheduleItemInfo>>
                ($"api/schedule/teacher/{teacherId}") ?? new();
        }

        public async Task<List<ScheduleItemInfo>>
            GetScheduleForClassroomAsync(int classroomId)
        {
            return await GetAsync<List<ScheduleItemInfo>>
                ($"api/schedule/classroom/{classroomId}") ?? new();
        }

        public async Task<List<ScheduleItemInfo>>
            GetScheduleForDepartmentAsync(int departmentId)
        {
            return await GetAsync<List<ScheduleItemInfo>>
                ($"api/schedule/department/{departmentId}") ?? new();
        }
    }
}
