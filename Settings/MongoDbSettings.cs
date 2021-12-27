namespace Catalog.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public TimeSpan TimeoutLimit { get; init; } = TimeSpan.FromSeconds(3);
        public string MongoDBName { get; init; } = "mongodb";

        public string ConnectionString 
        { 
            get
            {
                return $"mongodb://{User}:{Password}@{Host}:{Port}";
            }
        }
    }
}