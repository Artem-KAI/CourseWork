using CONTRACT.DTO;

using Microsoft.AspNetCore.Mvc;

using BLL.Exceptions;
using BLL.Interfaces;

using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleManager scheduleManager;

        public ScheduleController(IScheduleManager scheduleManager)
        {
            this.scheduleManager = scheduleManager;
        }

        private int CurrentUserId
        {
            get
            {
                if (Request.Headers.TryGetValue("X-User-Id", out var value))
                {
                    if (int.TryParse(value, out int id))
                    {
                        return id;
                    }
                }

                return 0;
            }
        }

        private ActionResult HandleException(Exception ex)
        {
            if (ex is AccessDeniedException)
            {
                return StatusCode(403, new
                {
                    Message = ex.Message
                });
            }

            if (ex is EntityNotFoundException)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }

            if (ex is ScheduleConflictException)
            {
                return Conflict(new
                {
                    Message = ex.Message
                });
            }

            if (ex is ArgumentException)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }

            return StatusCode(500, new
            {
                Message = ex.Message
            });
        }

        // Schedule endpoints
        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IReadOnlyCollection<ScheduleItemInfo>>>
            GetByGroup(int groupId)
        {
            try
            {
                var result = await scheduleManager
                    .GetScheduleForGroupAsync(
                    CurrentUserId,
                    groupId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<IReadOnlyCollection<ScheduleItemInfo>>>
            GetByTeacher(int teacherId)
        {
            try
            {
                var result = await scheduleManager
                    .GetScheduleForTeacherAsync(
                    CurrentUserId,
                    teacherId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("classroom/{classroomId}")]
        public async Task<ActionResult<IReadOnlyCollection<ScheduleItemInfo>>>
            GetByClassroom(int classroomId)
        {
            try
            {
                var result = await scheduleManager
                    .GetScheduleForClassroomAsync(
                    CurrentUserId,
                    classroomId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IReadOnlyCollection<ScheduleItemInfo>>>
            GetByDepartment(int departmentId)
        {
            try
            {
                var result = await scheduleManager
                    .GetScheduleForDepartmentAsync(
                    CurrentUserId,
                    departmentId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleItemInfo>> GetById(int id)
        {
            try
            {
                var result = await scheduleManager
                    .GetScheduleItemByIdAsync(
                    CurrentUserId,
                    id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ScheduleItemInfo>>> GetAll()
        {
            try
            {
                var result = await scheduleManager
                    .GetAllScheduleItemsAsync(CurrentUserId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateScheduleItemRequest request)
        {
            try
            {
                await scheduleManager
                    .CreateScheduleItemAsync(
                    CurrentUserId,
                    request.GroupId,
                    request.TeacherId,
                    request.ClassroomId,
                    request.DisciplineId,
                    request.DayOfWeek,
                    request.LessonNumber,
                    request.WeekType);

                return Ok(new
                {
                    Message = "Schedule item created."
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, 
            UpdateScheduleItemRequest request)
        {
            try
            {
                await scheduleManager
                    .UpdateScheduleItemAsync(
                    CurrentUserId,
                    id,
                    request.GroupId,
                    request.TeacherId,
                    request.ClassroomId,
                    request.DisciplineId,
                    request.DayOfWeek,
                    request.LessonNumber,
                    request.WeekType);

                return Ok(new
                {
                    Message = "Schedule item updated."
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await scheduleManager
                    .DeleteScheduleItemAsync(
                    CurrentUserId,
                    id);

                return Ok(new
                {
                    Message = "Schedule item deleted."
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // Reference lists endpoints
        [HttpGet("departments")]
        public async Task<ActionResult<IReadOnlyCollection<DepartmentInfo>>> Departments()
        {
            return Ok(await scheduleManager.GetAllDepartmentsAsync(CurrentUserId));
        }

        [HttpGet("groups")]
        public async Task<ActionResult<IReadOnlyCollection<GroupInfo>>> Groups()
        {
            return Ok(await scheduleManager.GetAllGroupsAsync(CurrentUserId));
        }

        [HttpGet("teachers")]
        public async Task<ActionResult<IReadOnlyCollection<TeacherInfo>>> Teachers()
        {
            return Ok(await scheduleManager.GetAllTeachersAsync(CurrentUserId));
        }

        [HttpGet("classrooms")]
        public async Task<ActionResult<IReadOnlyCollection<ClassroomInfo>>> Classrooms()
        {
            return Ok(await scheduleManager.GetAllClassroomsAsync(CurrentUserId));
        }

        [HttpGet("disciplines")]
        public async Task<ActionResult<IReadOnlyCollection<DisciplineInfo>>> Disciplines()
        {
            return Ok(await scheduleManager.GetAllDisciplinesAsync(CurrentUserId));
        }

        // Reference Create
        [HttpPost("departments")]
        public async Task<ActionResult> AddDepartment(CreateDepartmentRequest request)
        {
            try
            {
                await scheduleManager
                    .CreateDepartmentAsync(
                    CurrentUserId,
                    request.Name);

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("departments/{id}")]
        public async Task<ActionResult> UpdateDepartment(int id, UpdateDepartmentRequest request)
        {
            try
            {
                await scheduleManager.UpdateDepartmentAsync(CurrentUserId, id, request.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("departments/{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            try
            {
                await scheduleManager.DeleteDepartmentAsync(CurrentUserId, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("groups")]
        public async Task<ActionResult> AddGroup(CreateGroupRequest request)
        {
            try
            {
                await scheduleManager
                    .CreateGroupAsync(
                    CurrentUserId,
                    request.Name,
                    request.DepartmentId);

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("groups/{id}")]
        public async Task<ActionResult> UpdateGroup(int id, UpdateGroupRequest request)
        {
            try
            {
                await scheduleManager.UpdateGroupAsync(CurrentUserId, id, request.Name, request.DepartmentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("groups/{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            try
            {
                await scheduleManager.DeleteGroupAsync(CurrentUserId, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("teachers")]
        public async Task<ActionResult> AddTeacher(CreateTeacherRequest request)
        {
            try
            {
                await scheduleManager
                    .CreateTeacherAsync(
                    CurrentUserId,
                    request.Name,
                    request.Degree,
                    request.DepartmentId,
                    request.UserId);

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("teachers/{id}")]
        public async Task<ActionResult> UpdateTeacher(int id, UpdateTeacherRequest request)
        {
            try
            {
                await scheduleManager.UpdateTeacherAsync(CurrentUserId, id, request.Name, request.Degree, request.DepartmentId, request.UserId);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("teachers/{id}")]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
            try
            {
                await scheduleManager.DeleteTeacherAsync(CurrentUserId, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("classrooms")]
        public async Task<ActionResult> AddClassroom(CreateClassroomRequest request)
        {
            try
            {
                await scheduleManager
                    .CreateClassroomAsync(
                    CurrentUserId,
                    request.Name,
                    request.Building,
                    request.Capacity);

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("classrooms/{id}")]
        public async Task<ActionResult> UpdateClassroom(int id, UpdateClassroomRequest request)
        {
            try
            {
                await scheduleManager.UpdateClassroomAsync(CurrentUserId, id, request.Name, request.Building, request.Capacity);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("classrooms/{id}")]
        public async Task<ActionResult> DeleteClassroom(int id)
        {
            try
            {
                await scheduleManager.DeleteClassroomAsync(CurrentUserId, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("disciplines")]
        public async Task<ActionResult> AddDiscipline(CreateDisciplineRequest request)
        {
            try
            {
                await scheduleManager
                    .CreateDisciplineAsync(
                    CurrentUserId,
                    request.Name,
                    request.DepartmentId);

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("disciplines/{id}")]
        public async Task<ActionResult> UpdateDiscipline(int id, UpdateDisciplineRequest request)
        {
            try
            {
                await scheduleManager.UpdateDisciplineAsync(CurrentUserId, id, request.Name, request.DepartmentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("disciplines/{id}")]
        public async Task<ActionResult> DeleteDiscipline(int id)
        {
            try
            {
                await scheduleManager.DeleteDisciplineAsync(CurrentUserId, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
