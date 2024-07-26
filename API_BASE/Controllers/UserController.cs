using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_BASE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var items = await _userService.GetAllUsersAsync(false);
            return Ok(items);
        }

        [HttpGet("GetUserPermissions")]
        public async Task<IActionResult> GetUserPermissions([FromQuery] string query)
        {
            var userPermissions = await _userService.ExecuteSqlAsync<UserPermission>(query);
            return Ok(userPermissions);
        }
    }
}
