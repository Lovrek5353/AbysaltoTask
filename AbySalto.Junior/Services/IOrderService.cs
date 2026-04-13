using AbySalto.Junior.DTOs;
using AbySalto.Junior.Models;
using AbySalto.Junior.Models.Enums;

namespace AbySalto.Junior.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderDto dto);
        Task<IEnumerable<Order>> GetAllOrdersAsync(bool sortByAmount = false);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, OrderStatus newStatus);
    }
}
