using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Mentors.Domain.Entities.MongoDb
{
    public class MentorshipSubject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}