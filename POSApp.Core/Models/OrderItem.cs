namespace POSApp.Core.Models
{
    public class OrderItem
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Item.UnitPrice * Quantity;
    }
}