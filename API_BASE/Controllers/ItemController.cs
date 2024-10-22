using Application.Interfaces.Services;
using Application.Services;
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
        [HttpGet("GetItemById")]
        public async Task<IActionResult> GetItemById(int id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                if (item == null)
                {
                    throw new Exception("Not found Item with id " + id);
                }
                return Ok(new ApiResponse<Item>("success", "Record obtained correctly.", item));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", ex.Message, null));
            }

        }
        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem(Item model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Validation failed, review the model data");
                }
                await _itemService.AddItemAsync(model);
                return Ok(new ApiResponse<string>("success", "Record save correctly.", model.ItemCode));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", "An error occurred while adding the user, Error = " + ex.Message, null));
            }
        }
        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateItem([FromBody] Item item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Validation failed, review the model data");
                }

                var itemById = await _itemService.GetItemByIdAsync(item.Id_Item);
                if (itemById == null)
                {
                    throw new Exception("Item not found");
                }

                itemById.ItemCode = item.ItemCode;
                itemById.ItemName = item.ItemName;
                itemById.Modify_User = User.Identity.Name;
                itemById.Modify_Date = DateTime.Now;
                itemById.Active = item.Active;

                await _itemService.UpdateItemAsync(itemById);
                return Ok(new ApiResponse<int>("success", "Record updated correctly.", itemById.Id_Item));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("error", "An error occurred while adding the item. " + ex.Message, null));
            }
        }
    }
}
