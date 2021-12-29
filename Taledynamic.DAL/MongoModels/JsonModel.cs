using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Taledynamic.DAL.MongoModels
{
    public class JsonModel
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }        
        public BsonDocument Document { get; set; }
        public bool Immutable { get; set; }

    }
}