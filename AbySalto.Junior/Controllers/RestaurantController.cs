using AbySalto.Junior.DTOs;
using AbySalto.Junior.Models.Enums;
using AbySalto.Junior.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Junior.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public RestaurantController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            if (dto == null) return BadRequest("Podaci narudžbe su prazni.");

            var createdOrder = await _orderService.CreateOrderAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool sortByAmount = false)
        {
            var orders = await _orderService.GetAllOrdersAsync(sortByAmount);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatus newStatus)
        {
            var success = await _orderService.UpdateOrderStatusAsync(id, newStatus);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
