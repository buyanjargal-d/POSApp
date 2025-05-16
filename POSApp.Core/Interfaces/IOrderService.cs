using System.Collections.Generic;
using POSApp.Core.Models;

namespace POSApp.Core.Interfaces
{
    public interface IOrderService
    {
        void AddToOrder(string itemCode);
        void IncrementQuantity(string itemCode);
        void DecrementQuantity(string itemCode);
        decimal CalculateTotal();
        IEnumerable<OrderItem> GetOrderItems();
        void ClearOrder();
    }
}