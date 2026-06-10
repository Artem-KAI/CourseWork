using CONTRACT.DTO;

using Microsoft.AspNetCore.Mvc;

using BLL.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        private async Task<bool> IsAdmin()
        {
            if (!Request.Headers.TryGetValue("X-User-Id", out var value))
                return false;

            if (!int.TryParse(value, out int userId))
                return false;

            var users = await userManager.GetAllUsersAsync();
            var user = users.FirstOrDefault(x => x.Id == userId);

            return string.Equals(user?.Role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        [HttpPost]
        public async Task<ActionResult<UserInfo>> Create(CreateUserRequest request)
        {
            if (!await IsAdmin())
                return Forbid();

            var user = await userManager.CreateUserAsync(
                request.Username,
                request.Email,
                request.Password,
                request.Role);

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UserInfo>>> GetAll()
        {
            return Ok(await userManager.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> Get(int id)
        {
            var user = await userManager.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateUserRequests request)
        {
            if (!await IsAdmin())
                return Forbid();

            await userManager
                .UpdateUserAsync(
                id,
                request.Username,
                request.Email,
                request.Role);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await IsAdmin())
                return Forbid();

            await userManager.DeleteUserAsync(id);

            return Ok();
        }
    }
}
