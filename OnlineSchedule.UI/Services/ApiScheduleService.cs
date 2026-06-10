using CONTRACT.DTO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using UI.Models;

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

        public async Task<List<ScheduleItemInfo>> GetAllScheduleItemsAsync()
        {
            return await GetAsync<List<ScheduleItemInfo>>
                ("api/schedule") ?? new();
        }

        // Methods for Editor
        public async Task<ScheduleItemInfo?> GetScheduleItemByIdAsync(int id)
        {
            return await GetAsync<ScheduleItemInfo>(
                $"api/schedule/{id}");
        }

        public async Task<bool> CreateScheduleItemAsync(CreateScheduleItemRequest request)
        {
            var client = CreateClient();

            var response = await client.PostAsJsonAsync(
                    "api/schedule",
                    request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateScheduleItemAsync(
            int id,
            UpdateScheduleItemRequest request)
        {
            var client = CreateClient();

            var response = await client.PutAsJsonAsync(
                    $"api/schedule/{id}",
                    request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteScheduleItemAsync(int id)
        {
            var client = CreateClient();

            var response = await client.DeleteAsync(
                    $"api/schedule/{id}");

            return response.IsSuccessStatusCode;
        }

        // Methods for Admin
        public async Task<bool> AddDepartmentAsync(CreateDepartmentRequest request)
        {
            var client = CreateClient();

            var response = await client.PostAsJsonAsync(
                "api/schedule/departments",
                request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"api/schedule/departments/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/schedule/departments/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddGroupAsync(CreateGroupRequest request)
        {
            var client = CreateClient();

            var response = await client.PostAsJsonAsync(
                "api/schedule/groups",
                request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateGroupAsync(int id, UpdateGroupRequest request)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"api/schedule/groups/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/schedule/groups/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddTeacherAsync(CreateTeacherRequest request)
        {
            var client = CreateClient();

            var response = await client.PostAsJsonAsync(
                "api/schedule/teachers",
                request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTeacherAsync(int id, UpdateTeacherRequest request)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"api/schedule/teachers/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTeacherAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/schedule/teachers/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddClassroomAsync(CreateClassroomRequest request)
        {
            var client = CreateClient();

            var response = await client.PostAsJsonAsync(
                "api/schedule/classrooms",
                request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateClassroomAsync(int id, UpdateClassroomRequest request)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"api/schedule/classrooms/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteClassroomAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/schedule/classrooms/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddDisciplineAsync(CreateDisciplineRequest request)
        {
            var client = CreateClient();

            var response = await client.PostAsJsonAsync(
                "api/schedule/disciplines",
                request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDisciplineAsync(int id, UpdateDisciplineRequest request)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"api/schedule/disciplines/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDisciplineAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/schedule/disciplines/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
