using Catalog.Dtos;
using Catalog.Models;
using Catalog.Repositories;

namespace Catalog
{   
    public static class Extensions
    {
        public static IServiceCollection SetupApplicationSingletons(this IServiceCollection service){
            return service.AddSingleton<IItemsRepository, ItemsRepository>();
        }

        public static ItemDto AsDto(this Item item) {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate
            };            
        }
    }
}