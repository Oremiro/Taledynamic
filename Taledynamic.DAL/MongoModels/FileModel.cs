using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Taledynamic.DAL.MongoModels
{
    public class FileModel
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        // Занимает больше места чем Binary, но сейчас - все равно.
        public string FileBase64Data { get; set; } 
        public string Type { get; set; }
    }
}