using Catalog.Dtos;
using Catalog.Models;
using Catalog.Repositories;
using Catalog.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Catalog
{   
    public static class Extensions
    {
        public static WebApplicationBuilder SetupApplicationSingletons(this WebApplicationBuilder builder, string ConnectionString)
        {     
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));     
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));     
              
            builder.Services.AddSingleton<IMongoClient>(ServiceProvider => 
            {
                return new MongoClient(ConnectionString);
            });
            builder.Services.AddSingleton<IItemsRepository, MongoDBItemsRepository>();
            return builder;
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