namespace Taledynamic.DAL.Models.Internal
{
    public interface IMongoDbSettings
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
        
        public string JsonCollectionName { get; }
        public string TelegramCollectionName { get; }
        public string FileCollectionName { get; }
    }

    public class MongoDbSettings: IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string JsonCollectionName { get; set; }
        public string TelegramCollectionName { get; set; }
        public string FileCollectionName { get; set; }

    }
}