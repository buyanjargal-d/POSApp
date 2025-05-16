using System.Collections.Generic;
using POSApp.Core.Models;

namespace POSApp.Core.Interfaces
{
    public interface IItemService
    {
        IEnumerable<Item> GetAllItems();
        Item GetItemByCode(string code);
        void AddItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(int id);
    }
}