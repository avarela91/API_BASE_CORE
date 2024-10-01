using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace API_BASE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IDapperService _dapperService;
        private readonly IUserService _userService;

        public AccountController( IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUserPermissionsByUserAndModule")]
        public async Task<IActionResult> GetUserPermissions(string username, string moduleCode)
        {
            try
            {
                var permissions = await _userService.GetUserPermissionsAsync(username, moduleCode);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "error", Message = "An error occurred while updating the user", Error = ex.Message });
            }
        }
    }
}
