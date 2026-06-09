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
            await scheduleManager
                .CreateDepartmentAsync(
                CurrentUserId,
                request.Name);

            return Ok();
        }

        [HttpPost("groups")]
        public async Task<ActionResult> AddGroup(CreateGroupRequest request)
        {
            await scheduleManager
                .CreateGroupAsync(
                CurrentUserId,
                request.Name,
                request.DepartmentId);

            return Ok();
        }

        [HttpPost("teachers")]
        public async Task<ActionResult> AddTeacher(CreateTeacherRequest request)
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

        [HttpPost("classrooms")]
        public async Task<ActionResult> AddClassroom(CreateClassroomRequest request)
        {
            await scheduleManager
                .CreateClassroomAsync(
                CurrentUserId,
                request.Name,
                request.Building,
                request.Capacity);

            return Ok();
        }

        [HttpPost("disciplines")]
        public async Task<ActionResult> AddDiscipline(CreateDisciplineRequest request)
        {
            await scheduleManager
                .CreateDisciplineAsync(
                CurrentUserId,
                request.Name);

            return Ok();
        }
    }
}
