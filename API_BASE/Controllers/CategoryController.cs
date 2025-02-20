using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_BASE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(new ApiResponse<IEnumerable<Category>>("success", "Records obtained correctly", categories));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", ex.Message, null));
            }

        }
    }
}
