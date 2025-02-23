﻿using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using System.Linq.Expressions;

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

        [HttpPost("LoginApp")]
        public async Task<IActionResult> LoginApp(LoginModel loginModel)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (loginModel == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");
                if (String.IsNullOrEmpty(loginModel.UserName) || String.IsNullOrEmpty(loginModel.Password) || String.IsNullOrEmpty(loginModel.Module))
                    throw new Exception("El usuario, contraseña y modulo son obligatorios.");

                Expression<Func<User, bool>> predicate = u =>(u.UserName==loginModel.UserName) && (u.Password==loginModel.Password);

                var users = await _userService.GetUsersByConditionsAsync(predicate);
                if(!users.Any())
                {
                    throw new Exception("Nombre de usuario o contraseña incorrectos.");
                }
                var permissions = await _userService.GetUserPermissionsAsync(loginModel.UserName, loginModel.Module);
                return Ok(new ApiResponse<IEnumerable<UserPermission>>(
                            "success",
                            "Login exitoso. Permisos obtenidos correctamente.",
                            permissions
                        ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", ex.Message, null));
            }
        }

        [HttpGet("GetUserPermissionsByUserAndModule")]
        public async Task<IActionResult> GetUserPermissions(string username, string moduleCode)
        {
            try
            {
                var permissions = await _userService.GetUserPermissionsAsync(username, moduleCode);
                return Ok(new ApiResponse<IEnumerable<UserPermission>>("success", "Records obtained correctly",permissions));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", ex.Message, null));
            }
        }
    }
}
