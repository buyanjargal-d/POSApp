using System.Collections.Generic;
using System.Linq;
using POSApp.Core.Models;

namespace POSApp.Data.Repositories
{
    public class ItemRepository
    {
        private readonly DatabaseContext _context;

        public ItemRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<Item> GetAllItems()
        {
            return _context.Items.ToList();
        }

        public Item GetByCode(string code)
        {
            return _context.Items.FirstOrDefault(i => i.Code == code);
        }

        public void Add(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        public void Update(Item item)
        {
            _context.Items.Update(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
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
