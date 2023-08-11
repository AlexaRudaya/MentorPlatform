namespace Mentors.Infrastructure.Repositories
{
    public class CachedMentorRepository : IMentorRepository
    {
        private readonly IMentorRepository _decoratedRepository;
        private readonly IDistributedCache _distributedCache;

        public CachedMentorRepository(
            IMentorRepository decoratedRepository,
            IDistributedCache distributedCache)
        {
            _decoratedRepository = decoratedRepository;
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<Mentor>> GetAllByAsync(Func<IQueryable<Mentor>, 
            IIncludableQueryable<Mentor, object>> include = null,
            Expression<Func<Mentor, bool>> expression = null, 
            CancellationToken cancellationToken = default)
        {
            string key = "mentors";

            var cachedMentors = await _distributedCache.GetStringAsync(key);

            IEnumerable<Mentor> mentors;

            if (string.IsNullOrEmpty(cachedMentors))
            {
                mentors = await _decoratedRepository.GetAllByAsync(
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
                    SetExpirationTime(),
                    cancellationToken);

                return mentors;
            }

            mentors = JsonConvert.DeserializeObject<IEnumerable<Mentor>>(cachedMentors);

            return mentors;
        }

        public async Task<Mentor> GetOneByAsync(Func<IQueryable<Mentor>, 
            IIncludableQueryable<Mentor, object>> include = null, 
            Expression<Func<Mentor, bool>> expression = null, 
            CancellationToken cancellationToken = default)
        {
            string key = "mentor";

            var cachedMentor = await _distributedCache.GetStringAsync(key, cancellationToken);

            Mentor mentor;

            if (string.IsNullOrEmpty(cachedMentor))
            {
                mentor = await _decoratedRepository.GetOneByAsync(
                    include: query => query
                        .Include(mentor => mentor.Category)
                        .Include(mentor => mentor.Availabilities), 
                    expression, 
                    cancellationToken);

                if (mentor is null)
                {
                    throw new MentorNotFoundException();
                }

                var mentorJson = SerializeMentorToJson(mentor);

                await _distributedCache.SetStringAsync(key,
                    mentorJson,
                    SetExpirationTime(),
                    cancellationToken);

                return mentor;
            }

            mentor = JsonConvert.DeserializeObject<Mentor>(cachedMentor);

            return mentor;
        }

        public async Task CreateAsync(Mentor mentor, CancellationToken cancellationToken = default)
        {
            await _decoratedRepository.CreateAsync(mentor, cancellationToken);
        }

        public async Task UpdateAsync(Mentor mentor, CancellationToken cancellationToken = default)
        {
            await _decoratedRepository.UpdateAsync(mentor, cancellationToken);
        }

        public async Task DeleteAsync(Mentor mentor, CancellationToken cancellationToken = default)
        {
            await _decoratedRepository.DeleteAsync(mentor, cancellationToken);
        }

        private JsonSerializerSettings SerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return settings;
        }

        private string SerializeMentorsListToJson(IEnumerable<Mentor> mentors)
        {
            var settings = SerializerSettings();

            return JsonConvert.SerializeObject(mentors, settings);
        }

        private string SerializeMentorToJson(Mentor mentor)
        {
            var settings = SerializerSettings();

            return JsonConvert.SerializeObject(mentor, settings);
        }

        private DistributedCacheEntryOptions SetExpirationTime()
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };
        }
    }
}