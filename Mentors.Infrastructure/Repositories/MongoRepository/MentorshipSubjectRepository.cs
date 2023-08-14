using Mentors.Domain.Abstractions.IRepository.IMongoRepository;
using Mentors.Domain.Entities.MongoDb;
using Mentors.Infrastructure.MongoDb;
using Microsoft.Extensions.Options;

namespace Mentors.Infrastructure.Repositories.MongoRepository
{
    public class MentorshipSubjectRepository : MongoRepository<MentorshipSubject>, IMentorshipSubjectRepository
    {
        public MentorshipSubjectRepository(IOptions<MongoDbSettings> mongoSettings) : base(mongoSettings)
        {
        }
    }
}