using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Mentors.Infrastructure.Repositories
{
    public class CachedMentorRepository : ICachedMentorRepository
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly IDistributedCache _distributedCache;

        public CachedMentorRepository(
            IMentorRepository mentorRepository,
            IDistributedCache distributedCache)
        {
            _mentorRepository = mentorRepository;
            _distributedCache = distributedCache;
        }

        public Task<IEnumerable<Mentor>> GetAllByAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Mentor> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            string key = $"mentor-{id}";

            var cachedMentor = await _distributedCache.GetStringAsync(key, cancellationToken);

            Mentor mentor;

            if (string.IsNullOrEmpty(cachedMentor))
            {
                mentor = await _mentorRepository.GetOneByAsync(
                    include: query => query
                        .Include(mentor => mentor.Category)
                        .Include(mentor => mentor.Availabilities),
                    expression: mentor => mentor.Id.Equals(id),
                    cancellationToken: cancellationToken);

                if(mentor is null)
                {
                    throw new MentorNotFoundException(id);
                }

                var mentorJson = SerializeMentorToJson(mentor);

                await _distributedCache.SetStringAsync(key,
                   mentorJson,
                   cancellationToken);

                return mentor;
            }

            mentor = JsonConvert.DeserializeObject<Mentor>(cachedMentor);

            return mentor;
        }

        private string SerializeMentorToJson(Mentor mentor)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(mentor, settings);
        }
    }
}