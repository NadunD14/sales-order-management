using Microsoft.AspNetCore.Mvc;
using SalesOrderManagement.Application.Models;
using SalesOrderManagement.Application.Interfaces;

namespace SalesOrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems()
        {
            var items = await _itemRepository.GetAllAsync();
            var itemDtos = items.Select(i => new ItemDto
            {
                Id = i.Id,
                ItemCode = i.ItemCode,
                Description = i.Description,
                Price = i.Price
            });

            return Ok(itemDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var itemDto = new ItemDto
            {
                Id = item.Id,
                ItemCode = item.ItemCode,
                Description = item.Description,
                Price = item.Price
            };

            return Ok(itemDto);
        }
    }
}