using Catalog.Repositories;

namespace Catalog
{   
    public static class ApplicationSingletonExtension
    {
        public static IServiceCollection SetupApplicationSingletons(this IServiceCollection service){
            return service.AddSingleton<IItemsRepository, ItemsRepository>();
        }
    }
}