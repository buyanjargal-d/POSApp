using Xunit;
using Moq;
using System.Linq;
using POSApp.Core.Interfaces;
using POSApp.Core.Models;

namespace POSApp.Core.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IItemService> _itemServiceMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _itemServiceMock = new Mock<IItemService>();
            _orderService = new OrderService(_itemServiceMock.Object);
        }

        [Fact]
        public void AddToOrder_NewItem_AddsOnlyOne()
        {
            var item = new Item { Code = "X1", UnitPrice = 9.99m };
            _itemServiceMock.Setup(x => x.GetItemByCode("X1")).Returns(item);

            _orderService.AddToOrder("X1");
            var orderItems = _orderService.GetOrderItems().ToList();

            Assert.Single(orderItems);
            Assert.Equal("X1", orderItems[0].Item.Code);
            Assert.Equal(1, orderItems[0].Quantity);
        }

        [Fact]
        public void AddToOrder_SameItemTwice_IncrementsQuantity()
        {
            var item = new Item { Code = "X1", UnitPrice = 5m };
            _itemServiceMock.Setup(x => x.GetItemByCode("X1")).Returns(item);

            _orderService.AddToOrder("X1");
            _orderService.AddToOrder("X1");

            var orderItem = _orderService.GetOrderItems().Single();
            Assert.Equal(2, orderItem.Quantity);
        }

        [Fact]
        public void AddToOrder_InvalidCode()
        {
            _itemServiceMock.Setup(x => x.GetItemByCode(It.IsAny<string>())).Returns((Item)null);

            _orderService.AddToOrder("NOO");
            Assert.Empty(_orderService.GetOrderItems());
        }

        [Fact]
        public void IncrementQuantity_ExistingItem_IncreasesQuantity()
        {
            var item = new Item { Code = "Y2", UnitPrice = 1m };
            _itemServiceMock.Setup(x => x.GetItemByCode("Y2")).Returns(item);

            _orderService.AddToOrder("Y2"); 
            _orderService.IncrementQuantity("Y2");

            Assert.Equal(2, _orderService.GetOrderItems().Single().Quantity);
        }

        [Fact]
        public void IncrementQuantity_Nonexistent()
        {
            _orderService.IncrementQuantity("NONE");
            Assert.Empty(_orderService.GetOrderItems());
        }

        [Fact]
        public void DecrementQuantity_1_DecreasesQuantity()
        {
            var item = new Item { Code = "Z3", UnitPrice = 2m };
            _itemServiceMock.Setup(x => x.GetItemByCode("Z3")).Returns(item);

            _orderService.AddToOrder("Z3"); 
            _orderService.AddToOrder("Z3");
            _orderService.DecrementQuantity("Z3");

            Assert.Single(_orderService.GetOrderItems());
            Assert.Equal(1, _orderService.GetOrderItems().Single().Quantity);
        }

        [Fact]
        public void DecrementQuantity_0_RemovesItem()
        {
            var item = new Item { Code = "Z3", UnitPrice = 2m };
            _itemServiceMock.Setup(x => x.GetItemByCode("Z3")).Returns(item);

            _orderService.AddToOrder("Z3"); 
            _orderService.DecrementQuantity("Z3");

            Assert.Empty(_orderService.GetOrderItems());
        }

        [Fact]
        public void DecrementQuantity_Nonexistentg()
        {
            _orderService.DecrementQuantity("NONE");
            Assert.Empty(_orderService.GetOrderItems());
        }

        [Fact]
        public void CalculateTotal_MixedItems_ReturnsCorrectSum()
        {
            var itemA = new Item { Code = "A", UnitPrice = 2m };
            var itemB = new Item { Code = "B", UnitPrice = 3m };
            _itemServiceMock.Setup(x => x.GetItemByCode("A")).Returns(itemA);
            _itemServiceMock.Setup(x => x.GetItemByCode("B")).Returns(itemB);

            _orderService.AddToOrder("A");
            _orderService.AddToOrder("A");
            _orderService.AddToOrder("B");

            var total = _orderService.CalculateTotal();
            Assert.Equal(7m, total);
        }
    }
}
