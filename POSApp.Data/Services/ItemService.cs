using POSApp.Core.Interfaces;
using POSApp.Core.Models;
using POSApp.Data; // Adjust if your context is in another namespace
using System.Collections.Generic;
using System.Linq;

namespace POSApp.Core.Services
{
    public class ItemService : IItemService
    {
        private readonly DatabaseContext _context;

        public ItemService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Item> GetAllItems() => _context.Items.ToList();

        public Item GetItemByCode(string code) =>
            _context.Items.FirstOrDefault(i => i.Code == code);

        public void AddItem(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        public void UpdateItem(Item item)
        {
            _context.Items.Update(item);
            _context.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var item = _context.Items.Find(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}
