using CONTRACT.DTO;
using Microsoft.AspNetCore.Mvc;

using BLL.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ICredentialManager credentialManager;

        public AuthController(ICredentialManager credentialManager)
        {
            this.credentialManager = credentialManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserInfo>> Login(
            [FromBody] LoginRequest request)
        {
            try
            {
                var user = await credentialManager.AuthenticateAsync(
                    request.Email,
                    request.Password);

                if (user == null)
                {
                    return Unauthorized(
                        new
                        {
                            Message = "Invalid email or password."
                        });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        Message = ex.Message
                    });
            }
        }
    }
}
