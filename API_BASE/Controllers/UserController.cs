using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Domain.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var items = await _userService.GetAllUsersAsync(false);
            return Ok(items);
        }
       [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.AddUserAsync(model);

                return Ok(new {Status="success", Message = "User added successfully, UserId = "+ model.Id_User });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "error", Message = "An error occurred while adding the user, Error = " + ex.Message });
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userById = await _userService.GetUserByIdAsync(user.Id_User);
                if (user == null)
                {
                    return NotFound(new { Status = "error", Message = "User not found" });
                }

                userById.Name = user.Name;
                userById.UserName = user.UserName;
                userById.Password = user.Password;
                userById.Modify_User = user.Modify_User;
                userById.Modify_Date = user.Modify_Date;
                userById.Active = user.Active;

                await _userService.UpdateUserAsync(userById);

                return Ok(new { Status = "success", Message = "User updated successfully, UserId = "+ user.Id_User });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "error", Message = "An error occurred while updating the user", Error = ex.Message });
            }
        }
    }
}
