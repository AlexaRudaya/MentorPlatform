namespace Mentors.Infrastructure.Repositories
{
    public class CachedMentorRepository : IMentorRepository
    {
        public Task<Mentor> GetOneByAsync(Func<IQueryable<Mentor>, 
            IIncludableQueryable<Mentor, object>> include = null, 
            Expression<Func<Mentor, bool>> expression = null, 
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Mentor entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Mentor entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Mentor>> GetAllByAsync(Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>> include = null, Expression<Func<Mentor, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Mentor entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}