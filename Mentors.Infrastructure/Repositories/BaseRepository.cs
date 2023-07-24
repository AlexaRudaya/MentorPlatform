namespace Mentors.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly MentorDbContext _dbContext;
        private readonly DbSet<T> _table;

        public BaseRepository(
            MentorDbContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }


        public async Task<IEnumerable<T>> GetAllByAsync(Func<IQueryable<T>,
            IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _table;

            if (expression is not null)
            {
                query = query.Where(expression);
            }
            if (include is not null)
            {
                query = include(query);
            }

            return await query.AsNoTracking()
                              .ToListAsync(cancellationToken);
        }

        public async Task<T> GetOneByAsync(Func<IQueryable<T>,
            IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _table;

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (include is not null)
            {
                query = include(query);
            }

            var model = await query.AsNoTracking()
                                   .FirstOrDefaultAsync(cancellationToken);

            return model!;
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _table.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Attach(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _table.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}