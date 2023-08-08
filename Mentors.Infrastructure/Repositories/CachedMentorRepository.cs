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

        public async Task<IEnumerable<Mentor>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string key = "mentors";

            var cachedMentors = await _distributedCache.GetStringAsync(key);

            IEnumerable<Mentor> mentors;

            if (string.IsNullOrEmpty(cachedMentors))
            {
                mentors = await _mentorRepository.GetAllByAsync(
                    include: query => query
                        .Include(mentor => mentor.Category)
                        .Include(mentor => mentor.Availabilities),
                    cancellationToken: cancellationToken);

                if (mentors is null)
                {
                    throw new MentorNotFoundException();
                }

                var mentorsJson = SerializeMentorsListToJson(mentors);

                await _distributedCache.SetStringAsync(key,
                   mentorsJson,
                   new DistributedCacheEntryOptions
                   { 
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                   },
                   cancellationToken);

                return mentors;
            }

            mentors = JsonConvert.DeserializeObject<IEnumerable<Mentor>>(cachedMentors);

            return mentors;
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
                   new DistributedCacheEntryOptions
                   {
                       AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                   },
                   cancellationToken);

                return mentor;
            }

            mentor = JsonConvert.DeserializeObject<Mentor>(cachedMentor);

            return mentor;
        }

        private string SerializeMentorsListToJson(IEnumerable<Mentor> mentors)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(mentors, settings);
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