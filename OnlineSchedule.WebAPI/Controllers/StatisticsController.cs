using CONTRACT.DTO;

using Microsoft.AspNetCore.Mvc;

using BLL.Exceptions;
using BLL.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsManager statisticsManager;

        public StatisticsController(IStatisticsManager statisticsManager)
        {
            this.statisticsManager = statisticsManager;
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

        [HttpGet("teachers")]
        public async Task<ActionResult<IReadOnlyCollection<TeacherWorkloadDto>>>
            TeacherWorkload()
        {
            try
            {
                return Ok(
                    await statisticsManager.GetTeachersWorkloadAsync(
                        CurrentUserId));
            }
            catch (AccessDeniedException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpGet("classrooms")]
        public async Task<ActionResult<IReadOnlyCollection<ClassroomUtilizationDto>>>
            ClassroomUtilization()
        {
            try
            {
                return Ok(
                    await statisticsManager.GetClassroomsUtilizationAsync(
                        CurrentUserId));
            }
            catch (AccessDeniedException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpGet("groups")]
        public async Task<ActionResult<IReadOnlyCollection<GroupLoadDto>>> GroupLoad()
        {
            try
            {
                return Ok(
                    await statisticsManager.GetGroupsLoadAsync(
                        CurrentUserId));
            }
            catch (AccessDeniedException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }
    }
}
