using System.Net.Http.Json;
using CONTRACT.DTO;

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

        public async Task<List<GroupInfo>> GetGroupsAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<GroupInfo>>
                ("api/schedule/groups") ?? new();
        }

        public async Task<List<TeacherInfo>> GetTeachersAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<TeacherInfo>>
                ("api/schedule/teachers") ?? new();
        }

        public async Task<List<ClassroomInfo>> GetClassroomsAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<ClassroomInfo>>
                ("api/schedule/classrooms") ?? new();
        }

        public async Task<List<DepartmentInfo>> GetDepartmentsAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<DepartmentInfo>>
                ("api/schedule/departments") ?? new();
        }

        public async Task<List<DisciplineInfo>> GetDisciplinesAsync()
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<DisciplineInfo>>
                ("api/schedule/disciplines") ?? new();
        }

        public async Task<List<ScheduleItemInfo>>
            GetScheduleForGroupAsync(int groupId)
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<ScheduleItemInfo>>
                ($"api/schedule/group/{groupId}") ?? new();
        }

        public async Task<List<ScheduleItemInfo>>
            GetScheduleForTeacherAsync(int teacherId)
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<ScheduleItemInfo>>
                ($"api/schedule/teacher/{teacherId}") ?? new();
        }

        public async Task<List<ScheduleItemInfo>>
            GetScheduleForClassroomAsync(int classroomId)
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<ScheduleItemInfo>>
                ($"api/schedule/classroom/{classroomId}") ?? new();
        }

        public async Task<List<ScheduleItemInfo>>
            GetScheduleForDepartmentAsync(int departmentId)
        {
            var client = CreateClient();

            return await client.GetFromJsonAsync<List<ScheduleItemInfo>>
                ($"api/schedule/department/{departmentId}") ?? new();
        }
    }
}
