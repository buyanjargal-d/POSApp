using POSApp.Core.Interfaces;
using POSApp.Core.Models;

public class OrderService : IOrderService
{
    private readonly List<OrderItem> _orderItems = new();
    private readonly IItemService _itemService;

    public OrderService(IItemService itemService)
    {
        _itemService = itemService;
    }

    public void AddToOrder(string itemCode)
    {
        var existing = _orderItems.FirstOrDefault(x => x.Item.Code == itemCode);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            var item = _itemService.GetItemByCode(itemCode);
            if (item != null)
                _orderItems.Add(new OrderItem { Item = item, Quantity = 1 });
        }
    }

    public void IncrementQuantity(string itemCode)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.Item.Code == itemCode);
        if (orderItem != null)
            orderItem.Quantity++;
    }

    public void DecrementQuantity(string itemCode)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.Item.Code == itemCode);
        if (orderItem != null)
        {
            orderItem.Quantity--;
            if (orderItem.Quantity <= 0)
            {
                _orderItems.Remove(orderItem);
            }
        }
    }


    public decimal CalculateTotal()
    {
        return _orderItems.Sum(x => x.Total);
    }

    public IEnumerable<OrderItem> GetOrderItems() => _orderItems;

    public void ClearOrder() => _orderItems.Clear();
}