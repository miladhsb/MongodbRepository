using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongodbRepository.Entities
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
