using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_BASE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var items = await _itemService.GetAllItemsAsync();
                return Ok(new ApiResponse<IEnumerable<Item>>("success", "Records obtained correctly",items));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", ex.Message, null));
            }
            
        }
    }
}
