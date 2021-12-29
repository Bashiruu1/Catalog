using Catalog.Api.Dtos;
using Catalog.Api.Models;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Catalog.Api
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
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);          
        }
    }
}