using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongodbRepository.Entities
{
    public class Customer:BaseEntity
    {

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("contact")]
        public string Contact { get; set; }
        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}
