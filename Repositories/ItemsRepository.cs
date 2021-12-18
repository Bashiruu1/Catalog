using Catalog.Models;

namespace Catalog.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Magic Sword", Price = 40, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 25, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Bronze Field", Price = 20, CreatedDate = DateTimeOffset.UtcNow }
        };

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid id)
        {
            return items.Where(item => item.Id == id).SingleOrDefault();
        }

        public void CreateItem(Item item)
        {
            items.Add(item);
        }

        void IItemsRepository.UpdateItem(Item item)
        {
            var index = items.FindIndex(exstingItem => exstingItem.Id == item.Id);
            items[index] = item;
        }

        public void DeleteItem(Item item)
        {
            var index = items.FindIndex(exstingItem => exstingItem.Id == item.Id);
            items.RemoveAt(index);
        }
    }
}