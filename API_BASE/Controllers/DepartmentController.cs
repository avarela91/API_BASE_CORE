using Domain.Entities.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;

namespace API_BASE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();
                return Ok(new ApiResponse<IEnumerable<Department>>("success", "Records obtained correctly", departments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", ex.Message, null));
            }

        }
    }
}
