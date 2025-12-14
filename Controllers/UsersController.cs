using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (currentUserRole != "Accountant" && (currentUserIdClaim == null || int.Parse(currentUserIdClaim) != id))
            {
                return StatusCode(403, new { error = "You can only view your own account" });
            }

            var result = _userService.GetUserById(id);
            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpPut("{id}/block")]
        public IActionResult BlockUser(int id, [FromBody] Application.DTOs.User.BlockUserDto dto)
        {
            var result = _userService.BlockUser(id, dto?.BlockedUntil);
            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpPut("{id}/unblock")]
        public IActionResult UnblockUser(int id)
        {
            var result = _userService.UnblockUser(id);
            return Ok(result);
        }
    }
}
