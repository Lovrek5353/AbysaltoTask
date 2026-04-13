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
        private readonly ILogger<RestaurantController> _logger;

        public RestaurantController(IOrderService orderService, ILogger<RestaurantController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Request body was empty");
                return BadRequest("Request body cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(dto.CustomerName))
            {
                _logger.LogWarning("Customer name was missing");
                return BadRequest(new { Message = "Customer name is required." });
            }

            if (dto.Items == null || dto.Items.Count == 0)
            {
                _logger.LogWarning("Order creation failed: No items provided");
                return BadRequest(new { Message = "Order must have at least one item" });
            }

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
            if (id <= 0) return BadRequest("Invalid order ID");

            if (!Enum.IsDefined(typeof(OrderStatus), newStatus))
            {
                return BadRequest("Order status doesn't exist");
            }

            try
            {
                var success = await _orderService.UpdateOrderStatusAsync(id, newStatus);

                if (!success)
                    return NotFound(new { Message = $"Order with ID {id} was not found" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
