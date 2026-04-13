using AbySalto.Junior.Models.Enums;

namespace AbySalto.Junior.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string ContactNumber { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "EUR";

        public List<OrderItem> Items { get; set; } = new();

        public decimal TotalAmount => Items.Sum(x => x.Quantity * x.Price);
    }
}
