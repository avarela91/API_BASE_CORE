using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Domain.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Messaging;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            try
            {
                var items = await _userService.GetAllUsersAsync(false);
                return Ok(new ApiResponse<IEnumerable<User>>("success", "Records obtained correctly.", items));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", ex.Message, null));
            }
           
        }
       [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    throw new Exception("Not found User with id "+id);
                }
                return Ok(new ApiResponse<User>("success", "Records obtained correctly.", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", ex.Message, null));
            }
           
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(User model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Validation failed, review the model data");
                }            
                await _userService.AddUserAsync(model);
                return Ok(new ApiResponse<int>("success", "Records obtained correctly.", model.Id_User));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", "An error occurred while adding the user, Error = " + ex.Message, null));
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Validation failed, review the model data");
                }
            
                var userById = await _userService.GetUserByIdAsync(user.Id_User);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                userById.Name = user.Name;
                userById.UserName = user.UserName;
                userById.Password = user.Password;
                userById.Modify_User = user.Modify_User;
                userById.Modify_Date = user.Modify_Date;
                userById.Active = user.Active;

                await _userService.UpdateUserAsync(userById);
                return Ok(new ApiResponse<int>("success", "Records obtained correctly.", user.Id_User));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", "An error occurred while adding the user. "+ ex.Message, null));
            }
        }
    }
}
