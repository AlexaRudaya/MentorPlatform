namespace Mentors.Infrastructure.Repositories.MongoRepository
{
    public class MentorshipSubjectRepository : MongoRepository<MentorshipSubject>, IMentorshipSubjectRepository
    {
        public MentorshipSubjectRepository(IOptions<MongoDbSettings> mongoSettings) : base(mongoSettings)
        {
        }
    }
}