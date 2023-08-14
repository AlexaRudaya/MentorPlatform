using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Mentors.Domain.Entities.MongoDb
{
    public abstract class MongoBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}